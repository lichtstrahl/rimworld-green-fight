using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace GreenFight.World
{
    public class Page_GreenWorldParams : Page_CreateWorldParams
    {
        private const string _worldSeed = "GreenFight";
        private const float _planetCoverage = 0.1f;
        private const float NoPolution = 0.0f;
        
        private static float listingHeight;
        private static Vector2 scrollPosition;
        private static float warningHeight;


        public override void DoWindowContents(Rect rect)
        {
            this.DrawPageTitle(rect);
            Rect mainRect = this.GetMainRect(rect);
            float width1 = (float)(((double)mainRect.width - (double)this.Margin) * 0.5);
            Rect rect1 = new Rect(mainRect.x, mainRect.y, width1, mainRect.height);
            Widgets.BeginGroup(rect1);
            // Размер и seed планеты (фиксированы)
            Text.Font = GameFont.Small;
            float y1 = 0.0f;
            float width2 = rect1.width - 200f;
            Widgets.Label(new Rect(0.0f, y1, 200f, 30f), "WorldSeed".Translate());
            Widgets.Label(new Rect(200f, y1, width2, 30f), _worldSeed);
            float y2 = y1 + 40f;
            float y3 = y2 + 40f;
            Widgets.Label(new Rect(0.0f, y3, 200f, 30f), "PlanetCoverage".Translate());
            Rect rect2 = new Rect(200f, y3, width2, 30f);
            Widgets.ButtonText(rect2, _planetCoverage.ToStringPercent(), active: false);

            // Отключение блока с слайдерами
            // TooltipHandler.TipRegionByKey(new Rect(0.0f, y3, rect2.xMax, rect2.height), "PlanetCoverageTip");
            // float y4 = y3 + 40f;
            // Widgets.Label(new Rect(0.0f, y4, 200f, 30f), "PlanetRainfall".Translate());
            // this.rainfall = (OverallRainfall) Mathf.RoundToInt(Widgets.HorizontalSlider(new Rect(200f, y4, width2, 30f), (float) this.rainfall, 0.0f, (float) (OverallRainfallUtility.EnumValuesCount - 1), true, (string) "PlanetRainfall_Normal".Translate(), (string) "PlanetRainfall_Low".Translate(), (string) "PlanetRainfall_High".Translate(), 1f));
            // float y5 = y4 + 40f;
            // Widgets.Label(new Rect(0.0f, y5, 200f, 30f), "PlanetTemperature".Translate());
            // this.temperature = (OverallTemperature) Mathf.RoundToInt(Widgets.HorizontalSlider(new Rect(200f, y5, width2, 30f), (float) this.temperature, 0.0f, (float) (OverallTemperatureUtility.EnumValuesCount - 1), true, (string) "PlanetTemperature_Normal".Translate(), (string) "PlanetTemperature_Low".Translate(), (string) "PlanetTemperature_High".Translate(), 1f));
            // float y6 = y5 + 40f;
            // Widgets.Label(new Rect(0.0f, y6, 200f, 30f), "PlanetPopulation".Translate());
            // this.population = (OverallPopulation) Mathf.RoundToInt(Widgets.HorizontalSlider(new Rect(200f, y6, width2, 30f), (float) this.population, 0.0f, (float) (OverallPopulationUtility.EnumValuesCount - 1), true, (string) "PlanetPopulation_Normal".Translate(), (string) "PlanetPopulation_Low".Translate(), (string) "PlanetPopulation_High".Translate(), 1f));
            // if (ModsConfig.BiotechActive)
            // {
            //   float y7 = y6 + 40f;
            //   Widgets.Label(new Rect(0.0f, y7, 200f, 30f), "PlanetPollution".Translate());
            //   this.pollution = Widgets.HorizontalSlider(new Rect(200f, y7, width2, 30f), this.pollution, 0.0f, 1f, true, this.pollution.ToStringPercent(), roundTo: 0.05f);
            // }
            Widgets.EndGroup();


            Rect rect3 = new Rect(mainRect.x + mainRect.xMax - width1, mainRect.y, width1, mainRect.height);
            DrawFactionsSettings(rect3, DefaultFactions(), true);
            float y8 = rect.yMax - 38f;
            float x = mainRect.center.x;
            Rect rect4 = new Rect((float)((double)x - (double)BottomButSize.x - 8.5), y8, BottomButSize.x,
                BottomButSize.y);
            rect4.x = x + 8.5f;

            this.DoBottomButtons(rect, (string)"WorldGenerate".Translate());
        }

        // Переход на следующий экран только после завершения генерации мира. Принудительно закрываем окно и открываем следующее.
        protected override bool CanDoNext()
        {
            LongEventHandler.QueueLongEvent(() =>
            {
                Find.GameInitData.ResetWorldRelatedMapInitData();
                Current.Game.World = WorldGenerator.GenerateWorld(
                    _planetCoverage,
                    _worldSeed,
                    OverallRainfall.Normal,
                    OverallTemperature.Normal,
                    OverallPopulation.Normal,
                    null,
                    NoPolution
                );

                LongEventHandler.ExecuteWhenFinished(() =>
                {
                    Find.WindowStack.Add(next);
                    MemoryUtility.UnloadUnusedUnityAssets();
                    Find.World.renderer.RegenerateAllLayersNow();
                    Close();
                });
            }, "GeneratingWorld", true, null);

            return false;
        }

        // private

        private List<FactionDef> DefaultFactions()
        {
            var factions = new List<FactionDef>();
            foreach (FactionDef configurableFaction in FactionGenerator.ConfigurableFactions)
            {
                if (configurableFaction.startingCountAtWorldCreation > 0)
                {
                    for (int index = 0; index < configurableFaction.startingCountAtWorldCreation; ++index)
                        factions.Add(configurableFaction);
                }
            }

            foreach (FactionDef configurableFaction in FactionGenerator.ConfigurableFactions)
            {
                FactionDef faction = configurableFaction;
                if (faction.replacesFaction != null)
                    factions.RemoveAll(x => x == faction.replacesFaction);
            }

            return factions;
        }

        private static void DrawFactionsSettings(
            Rect rect,
            List<FactionDef> factions,
            bool isDefaultFactionCounts)
        {
            Rect rect1 = new Rect(rect.x, rect.y, rect.width, Text.LineHeight);
            Widgets.Label(rect1, "Factions".Translate());
            TooltipHandler.TipRegion(rect1,
                (Func<string>)(() => (string)"FactionSelectionDesc".Translate((NamedArgument)12)), 4534123);
            float num1 = Text.LineHeight + 4f;
            float num2 = rect.width * 0.050000012f;
            Rect rect2 = new Rect(rect.x + num2, rect.y + num1, rect.width * 0.9f,
                (float)((double)rect.height - (double)num1 - (double)Text.LineHeight - 28.0) -
                warningHeight);
            Rect outRect = rect2.ContractedBy(4f);
            Rect rect3 = new Rect(outRect.x, outRect.y, outRect.width, listingHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, rect3);
            listingHeight = 0.0f;
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.ColumnWidth = rect3.width;
            listingStandard.Begin(rect3);
            for (int index = 0; index < factions.Count; ++index)
            {
                if (factions[index].displayInFactionSelection)
                {
                    listingStandard.Gap(4f);
                    if (DoRow(listingStandard.GetRect(24f), factions[index], factions, index))
                        --index;
                    listingStandard.Gap(4f);
                    listingHeight += 32f;
                }
            }

            listingStandard.End();
            Widgets.EndScrollView();
            Rect rect4 = new Rect(outRect.x,
                Mathf.Min(rect2.yMax, (float)((double)outRect.y + (double)listingHeight + 4.0)),
                outRect.width, 28f);

            

            float yMax = rect4.yMax;
            int num4 = factions.Count<FactionDef>((Predicate<FactionDef>)(x => !x.hidden));
            StringBuilder stringBuilder = new StringBuilder();
            if (num4 == 0)
            {
                stringBuilder.AppendLine((string)"FactionsDisabledWarning".Translate());
            }
            else
            {
                if (ModsConfig.RoyaltyActive && !factions.Contains(FactionDefOf.Empire))
                    stringBuilder.AppendLine((string)("Warning".Translate() + ": " +
                                                      "FactionDisabledContentWarning".Translate(
                                                          (NamedArgument)FactionDefOf.Empire.label)));
                if (!factions.Contains(FactionDefOf.Mechanoid))
                    stringBuilder.AppendLine((string)("Warning".Translate() + ": " +
                                                      "MechanoidsDisabledContentWarning".Translate(
                                                          (NamedArgument)FactionDefOf.Mechanoid.label)));
                if (!factions.Contains(FactionDefOf.Insect))
                    stringBuilder.AppendLine((string)("Warning".Translate() + ": " +
                                                      "InsectsDisabledContentWarning".Translate(
                                                          (NamedArgument)FactionDefOf.Insect.label)));
            }

            warningHeight = 0.0f;
            if (stringBuilder.Length <= 0)
                return;
            int num5 = Text.WordWrap ? 1 : 0;
            string str1 = stringBuilder.ToString().TrimEndNewlines();
            Rect rect5 = new Rect(rect.x, yMax, rect.width, rect.yMax - yMax);
            GUI.color = Color.yellow;
            Text.Font = GameFont.Tiny;
            warningHeight = Text.CalcHeight(str1, rect5.width);
            Text.WordWrap = true;
            Widgets.Label(rect5, str1);
            Text.WordWrap = num5 != 0;
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
        }

        private static bool DoRow(Rect rect, FactionDef factionDef, List<FactionDef> factions, int index)
        {
            bool flag = false;
            Rect rect1 = new Rect(rect.x, rect.y - 4f, rect.width, rect.height + 8f);
            if (index % 2 == 1)
                Widgets.DrawLightHighlight(rect1);
            Widgets.BeginGroup(rect);
            WidgetRow widgetRow = new WidgetRow(6f, 0.0f);
            GUI.color = factionDef.DefaultColor;
            widgetRow.Icon((Texture)factionDef.FactionIcon);
            GUI.color = Color.white;
            widgetRow.Gap(4f);
            Text.Anchor = TextAnchor.MiddleCenter;
            widgetRow.Label((string)factionDef.LabelCap);
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.EndGroup();
            if (Mouse.IsOver(rect1))
            {
                TooltipHandler.TipRegion(rect1,
                    (TipSignal)$"{factionDef.LabelCap.AsTipTitle()}\n{factionDef.Description}");
                Widgets.DrawHighlight(rect1);
            }

            return flag;
        }
    }
}