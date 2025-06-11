using System.Linq;
using GreenFight.Scenario;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GreenFight.Harmony
{

    [StaticConstructorOnStartup]
    public static class AutoStartPatch
    {
        static AutoStartPatch()
        {
            var harmony = new HarmonyLib.Harmony("greenfight.autostart");
            Log.Message("Применение Harmony патчей");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
        public static class Patch_Page_SelectScenario_BeginScenarioConfiguration
        {
            public static bool Prefix(RimWorld.Scenario scen, Page originPage)
            {
                Log.Message($"Перехват BeginScenarioConfiguration({scen.name}, {originPage.PageTitle})");
                Origin(scen, originPage);
                return false;
            }

            private static void Origin(RimWorld.Scenario scenario, Page page)
            {
                Current.Game = new Game();
                Current.Game.InitData = new GameInitData();
                Current.Game.Scenario = scenario;
                Current.Game.Scenario.PreConfigure();
                Find.GameInitData.startedFromEntry = true;
                Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
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