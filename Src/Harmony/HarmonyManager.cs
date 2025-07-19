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
            LogPatch(Category.WorldGeneration, isEnable);
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
            LogPatch(Category.SelectStartingSite, isEnable);
            if (isEnable)
            {
                Harmony.PatchCategory(nameof(Category.SelectStartingSite));
            }
            else
            {
                Harmony.UnpatchCategory(nameof(Category.SelectStartingSite));
            }
        }

        public static void PatchThought(bool isEnable)
        {
            LogPatch(Category.Thought, isEnable);
            if (isEnable)
            {
                Harmony.PatchCategory(nameof(Category.Thought));
            }
            else
            {
                Harmony.UnpatchCategory(nameof(Category.Thought));
            }
        }
        
        public enum Category
        {
            Scenario,
            WorldGeneration,
            SelectStartingSite,
            // Мысли/Моральные убеждения
            Thought
        }
        
        //private

        private static void LogPatch(Category category, bool isEnable)
        {
            Log.Message($"Harmony патч {nameof(category)}: {isEnable}");
        }
        
    }
}