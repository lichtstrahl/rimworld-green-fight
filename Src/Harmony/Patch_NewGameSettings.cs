using System;
using System.Collections.Generic;
using System.Linq;
using GreenFight.Scenario;
using GreenFight.Storyteller;
using GreenFight.World;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace GreenFight.Harmony
{

    // Изменение процесса настройки новой игры
    public static class Patch_NewGameSettings
    {
        [HarmonyPatchCategory(nameof(HarmonyManager.Category.Scenario))]
        [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
        public static class Patch_Page_SelectScenario_BeginScenarioConfiguration
        {
            private const string _targetScenarioName = "GreenFight";
            private const int _DefaultStartingTileId = 53223;
            
            public static bool Prefix(RimWorld.Scenario scen, Page originPage)
            {
                if (scen.name == _targetScenarioName)
                {
                    Log.Message($"Выбран {_targetScenarioName} сценарий.");
                    
                    Origin(scen, originPage);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // scenario взят из XML и не имеет каких-либо кастомных вещей. Метод GetFirstConfigPage переопределен отдельно.
            private static void Origin(RimWorld.Scenario scenario, Page page)
            {
                Current.Game = new Game();
                Current.Game.InitData = new GameInitData();
                Current.Game.Scenario = scenario;
                Current.Game.Scenario.PreConfigure();
                Find.GameInitData.startedFromEntry = true;
                // По умолчанию игра работает в "Ответственном режиме"
                Find.GameInitData.permadeath = true;
                Find.GameInitData.permadeathChosen = true;
                
                Page firstConfigPage = GetFirstConfigPage();
                if (firstConfigPage == null)
                {
                    PageUtility.InitGameStart();
                }
                else
                {
                    page.next = firstConfigPage;
                    firstConfigPage.prev = page;
                }
            }

            private static Page GetFirstConfigPage()
            {
                List<Page> pageList = new List<Page>();
                pageList.Add(new Page_GreenStoryteller(true, _DefaultStartingTileId));
                // pageList.Add(new Page_GreenWorldParams());
                // pageList.Add(new Page_SelectStartingSite());
                
                Page firstConfigPage = PageUtility.StitchedPages(pageList);
                if (firstConfigPage != null)
                {
                    Page page = firstConfigPage;
                    while (page.next != null)
                        page = page.next;
                    page.nextAct = () => PageUtility.InitGameStart();
                }
                return firstConfigPage;
            }
        }
}
}