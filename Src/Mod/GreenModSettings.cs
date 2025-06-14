﻿using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Mod
{
    
    // На момент инициализации настроек дефы ещё могут быть не загружены. Поэтому погоду подгружаем только при обращении.
    public class GreenModSettings : ModSettings
    {
        private static string _languageKey = "GreenModSettings";
        
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

        public int lightningCount = 0;
        private string lightningCountBuff;

        public int foodSatisfyingPower;
        private string _foodSatisfyingPowerBuff;
        
        public override void ExposeData()
        {
            Scribe_Values.Look<string>(ref _weatherDefName, "weatherDefName");
            Scribe_Values.Look<IntRange>(ref RaidPowerRange, "RaidPowerRange", new IntRange(100, 500));
            Scribe_Values.Look(ref lightningCount, "lightningCount", 0);
            Scribe_Values.Look(ref foodSatisfyingPower, "foodSatisfyingPower", 100);
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

            if (listing.ButtonText($"{_languageKey}_SelectWeather".Translate(WeatherDef.LabelCap)))
            {
                var options = DefDatabase<WeatherDef>.AllDefsListForReading
                    .Select(weather => new FloatMenuOption(weather.LabelCap, () =>
                    {
                        _weatherDefName = weather.defName;
                        _weather = null;
                    }));
                Find.WindowStack.Add(new FloatMenu(options.ToList()));
            }

            listing.Label($"{_languageKey}_raidPowerRange".TranslateSimple());
            listing.IntRange(ref RaidPowerRange, 1, 10_000);
            listing.TextFieldNumericLabeled($"{_languageKey}_lightningCount".TranslateSimple(), ref lightningCount, ref lightningCountBuff, 0, 1000);
            listing.TextFieldNumericLabeled($"{_languageKey}_foodSatisfying".TranslateSimple(), ref foodSatisfyingPower, ref _foodSatisfyingPowerBuff, 50, 50_000);
            
            listing.End();
            _viewHeight = listing.CurHeight;
            
            Widgets.EndScrollView();
        }
    }
}