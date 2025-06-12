using System.Reflection;
using Verse;

namespace GreenFight.Harmony
{
    [StaticConstructorOnStartup]
    public static class HarmonyStartup
    {
        public static readonly HarmonyLib.Harmony Harmony = new HarmonyLib.Harmony("greenfight.autostart");
        
        static HarmonyStartup()
        {
            Log.Message("Применение Harmony патчей");
            // HarmonyLib.Harmony.DEBUG = true;
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        
        public enum Category
        {
            Scenario
        }
        
    }
}