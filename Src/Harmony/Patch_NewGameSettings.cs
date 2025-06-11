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
        [HarmonyPatchCategory(nameof(HarmonyStartup.Category.Scenario))]
        [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
        public static class Patch_Page_SelectScenario_BeginScenarioConfiguration
        {
            private const string _targetScenarioName = "GreenFight";
            
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
                pageList.Add(new Page_GreenStoryteller());
                pageList.Add(new Page_GreenWorldParams());
                pageList.Add(new Page_SelectStartingSite());
                
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

/**
using HarmonyLib;
using RimWorld;
using Verse;

namespace YourModNamespace
{
    [StaticConstructorOnStartup]
    public static class AutoStartHarmonyPatch
    {
        static AutoStartHarmonyPatch()
        {
            var harmony = new Harmony("yourname.rimworld.autostart");
            harmony.PatchAll();
            Log.Message("[AutoStart] Harmony патчи применены.");
        }

        [HarmonyPatch(typeof(Page_SelectScenario), "DoNext")]
        public static class Patch_Page_SelectScenario_DoNext
        {
            public static bool Prefix(Page_SelectScenario __instance)
            {
                if (__instance.selectedScenario == null)
                {
                    Log.Error("[AutoStart] Сценарий не выбран.");
                    return true; // Пусть игра продолжит обычный процесс
                }

                Log.Message("[AutoStart] Перехвачено DoNext(). Запускаем автоматическую игру.");

                // Запуск ручной генерации игры
                AutoStartGameUtility.StartNewGameAuto(__instance.selectedScenario);

                // Отменяем переход к следующей странице
                return false;
            }
        }
    }
}
*/