using Verse;

namespace GreenFight.Harmony
{
    [StaticConstructorOnStartup]
    public static class HarmonyStartup
    {
        static HarmonyStartup()
        {
            Log.Message("Применение Harmony патчей");
            var harmony = new HarmonyLib.Harmony("greenfight.autostart");
            harmony.PatchAll();
        }
    }
}