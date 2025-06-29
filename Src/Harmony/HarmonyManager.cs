using System.Reflection;
using Verse;

namespace GreenFight.Harmony
{
    [StaticConstructorOnStartup]
    public static class HarmonyManager
    {
        public static readonly HarmonyLib.Harmony Harmony = new HarmonyLib.Harmony("greenfight.autostart");
        
        static HarmonyManager()
        {
            Log.Message("Применение Harmony патчей");
            // HarmonyLib.Harmony.DEBUG = true;
            Harmony.PatchCategory(nameof(Category.Scenario));
        }

        public static void PatchWorldGeneration(bool isEnable)
        {
            if (isEnable)
            {
                Harmony.PatchCategory(nameof(Category.WorldGeneration));
            }
            else
            {
                Harmony.UnpatchCategory(nameof(Category.WorldGeneration));
            }
        }

        public static void PatchSelectStartingSite(bool isEnable)
        {
            if (isEnable)
            {
                Harmony.PatchCategory(nameof(Category.SelectStartingSite));
            }
            else
            {
                Harmony.UnpatchCategory(nameof(Category.SelectStartingSite));
            }
        }
        
        
        public enum Category
        {
            Scenario,
            WorldGeneration,
            SelectStartingSite
        }
        
    }
}