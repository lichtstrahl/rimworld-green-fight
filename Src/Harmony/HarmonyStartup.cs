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
            Harmony.PatchCategory(nameof(Category.Scenario));
        }
        
        public enum Category
        {
            Scenario
        }
        
    }
}