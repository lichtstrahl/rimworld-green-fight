using UnityEngine;
using Verse;

namespace GreenFight.Mod
{
    public class GreenMod : Verse.Mod
    {
        public static GreenModSettings Settings { get; private set; }
        
        public GreenMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<GreenModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Green Fight";
        }
    }
}