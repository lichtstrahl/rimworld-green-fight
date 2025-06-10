using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Mod
{
    public class GreenModSettings : ModSettings
    {
        private static string _LanguageKey = "GreenModSettings";
        
        private string _weatherDefName;
        private WeatherDef _weather;

        public WeatherDef WeatherDef
        {
            get
            {
                if (_weather == null)
                {
                    if (!_weatherDefName.NullOrEmpty())
                    {
                        _weather = DefDatabase<WeatherDef>.GetNamed(_weatherDefName);
                    }
                    else
                    {
                        _weather = WeatherDefOf.Clear;
                    }
                }
                
                return _weather;
            }
        }

        public IntRange RaidPowerRange = new IntRange(100, 500);
        private Vector2 _scrollPosition = Vector2.zero;
        private float _viewHeight = 0;

        public int lightningCount = 1;
        private string lightningCountBuff;
        
        public override void ExposeData()
        {
            Scribe_Values.Look<string>(ref _weatherDefName, "weatherDefName");
            Scribe_Values.Look<IntRange>(ref RaidPowerRange, "RaidPowerRange", new IntRange(100, 500));
            Scribe_Values.Look(ref lightningCount, "lightningCount", 1);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect viewRect = new Rect(0, 0, inRect.width - 16f, _viewHeight);
            Widgets.BeginScrollView(inRect, ref _scrollPosition, viewRect);

            Listing_Standard listing = new Listing_Standard()
            {
                ColumnWidth = inRect.width
            };
            Rect listingRect = inRect.AtZero();
            listingRect.height = 99_999f;
            listing.Begin(listingRect);

            if (listing.ButtonText($"{_LanguageKey}_SelectWeather".Translate(WeatherDef.LabelCap)))
            {
                var options = DefDatabase<WeatherDef>.AllDefsListForReading
                    .Select(weather => new FloatMenuOption(weather.LabelCap, () =>
                    {
                        _weatherDefName = weather.defName;
                        _weather = null;
                    }));
                Find.WindowStack.Add(new FloatMenu(options.ToList()));
            }

            listing.IntRange(ref RaidPowerRange, 1, 10_000);
            listing.TextFieldNumeric(ref lightningCount, ref lightningCountBuff, 1, 1000);
            
            listing.End();
            _viewHeight = listing.CurHeight;
            
            Widgets.EndScrollView();
        }
    }
}