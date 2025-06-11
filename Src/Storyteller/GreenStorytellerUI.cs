using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Storyteller
{
    [StaticConstructorOnStartup]
    public static class GreenStorytellerUI
    {
        private static Vector2 scrollPosition = new Vector2();
        private static Vector2 explanationScrollPosition = new Vector2();
        private static AnimationCurve explanationScrollPositionAnimated;
        private static Rect explanationInnerRect = new Rect();
        // private static float sectionHeightThreats = 0.0f;
        // private static float sectionHeightGeneral = 0.0f;
        // private static float sectionHeightPlayerTools = 0.0f;
        // private static float sectionHeightEconomy = 0.0f;
        // private static float sectionHeightAdaptation = 0.0f;
        // private static float sectionHeightIdeology = 0.0f;
        // private static float sectionHeightChildren = 0.0f;
        // private static float sectionHeightAnomaly = 0.0f;

        private static readonly Texture2D StorytellerHighlightTex =
            ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight");

        private const float CustomSettingsPrecision = 0.01f;

        private static readonly Vector2 PortraitSizeTiny = new Vector2(122f, 130f);
        private static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

        public static void DrawStorytellerSelectionInterface(
            Rect rect,
            StorytellerDef storyteller,
            List<DifficultyDef> availableDifficulties,
            ref DifficultyDef difficulty,
            ref Difficulty difficultyValues,
            Listing_Standard infoListing)
        {
            Widgets.BeginGroup(rect);
            Rect outRect1 = new Rect(0.0f, 0.0f, PortraitSizeTiny.x + 16f, rect.height);
            Rect viewRect = new Rect(0.0f, 0.0f, PortraitSizeTiny.x,
                (float)DefDatabase<StorytellerDef>.AllDefs.Count<StorytellerDef>() *
                (PortraitSizeTiny.y + 10f));
            // Список рассказчиков.
            Widgets.BeginScrollView(outRect1, ref scrollPosition, viewRect);
            Rect rect1 = new Rect(0.0f, 0.0f, PortraitSizeTiny.x, PortraitSizeTiny.y)
                .ContractedBy(4f);
            bool selected = true;
            Widgets.DrawOptionBackground(rect1, selected);
            if (Widgets.ButtonImage(rect1, storyteller.portraitTinyTex, Color.white,
                    new Color(0.72f, 0.68f, 0.59f)))
            {
                TutorSystem.Notify_Event((EventPack)"ChooseStoryteller");
            }

            if (selected)
                GUI.DrawTexture(rect1, (Texture)StorytellerHighlightTex);
            rect1.y += rect1.height + 8f;


            Widgets.EndScrollView();
            Rect outRect2 = new Rect(outRect1.xMax + 8f, 0.0f,
                (float)((double)rect.width - (double)outRect1.width - 8.0), rect.height);
            explanationInnerRect.width = outRect2.width - 16f;
            Widgets.BeginScrollView(outRect2, ref explanationScrollPosition,
                explanationInnerRect);
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(0.0f, 0.0f, 300f, 999f), "HowStorytellersWork".Translate());
            Rect rect2 = new Rect(0.0f, 120f, 290f, 9999f);
            float val2 = 300f;

            // Основное описание рассказчика
            Rect position = new Rect(390f - outRect2.x,
                (float)((double)rect.height - (double)PortraitSizeLarge.y - 1.0),
                PortraitSizeLarge.x, PortraitSizeLarge.y);
            GUI.DrawTexture(position, (Texture)storyteller.portraitLargeTex);
            Text.Anchor = TextAnchor.UpperLeft;
            infoListing.Begin(rect2);
            Text.Font = GameFont.Medium;
            infoListing.Indent(15f);
            infoListing.Label(storyteller.label);
            infoListing.Outdent(15f);
            Text.Font = GameFont.Small;
            infoListing.Gap(8f);
            infoListing.Label(storyteller.description, 160f);
            infoListing.Gap(6f);
            foreach (DifficultyDef allDef in availableDifficulties)
            {
                TaggedString labelCap = allDef.LabelCap;
                if (allDef.isCustom)
                    labelCap += "...";
                if (infoListing.RadioButton((string)labelCap, difficulty == allDef,
                        tooltip: allDef.description.ResolveTags(), tooltipDelay: new float?(0.0f)))
                {
                    if (!allDef.isCustom)
                        difficultyValues.CopyFrom(allDef);
                    else if (allDef != difficulty)
                    {
                        difficultyValues.CopyFrom(DifficultyDefOf.Rough);
                        float time = Time.time;
                        float num = 0.6f;
                        explanationScrollPositionAnimated = AnimationCurve.EaseInOut(time,
                            explanationScrollPosition.y, time + num,
                            explanationInnerRect.height);
                    }

                    difficulty = allDef;
                }

                infoListing.Gap(3f);
            }

            if (Current.ProgramState == ProgramState.Entry)
            {
                infoListing.Gap(15f);
                bool active1 = Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
                bool active2 = Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
                if (infoListing.RadioButton((string)"ReloadAnytimeMode".Translate(), active2,
                        tooltip: (string)"ReloadAnytimeModeInfo".Translate()))
                {
                    Find.GameInitData.permadeathChosen = true;
                    Find.GameInitData.permadeath = false;
                }

                infoListing.Gap(3f);
                if (infoListing.RadioButton(
                        (string)"CommitmentMode".TranslateWithBackup((TaggedString)"PermadeathMode"), active1,
                        tooltip: (string)"PermadeathModeInfo".Translate()))
                {
                    Find.GameInitData.permadeathChosen = true;
                    Find.GameInitData.permadeath = true;
                }

                if (ModsConfig.AnomalyActive)
                {
                    infoListing.Gap(15f);
                    if (infoListing.ButtonText((string)("AnomalySettings".Translate() + "...")))
                    {
                        if (difficulty == null)
                            Messages.Message((string)"MustChooseDifficulty".Translate(),
                                MessageTypeDefOf.RejectInput, false);
                        else
                            Find.WindowStack.Add((Verse.Window)new Dialog_AnomalySettings(difficultyValues));
                    }
                }
            }

            val2 = rect2.y + infoListing.CurHeight;
            infoListing.End();
            if (difficulty != null && difficulty.isCustom)
            {
                if (explanationScrollPositionAnimated != null)
                {
                    float time = Time.time;
                    if ((double)time <
                        (double)((IEnumerable<UnityEngine.Keyframe>)explanationScrollPositionAnimated
                            .keys).Last<UnityEngine.Keyframe>().time)
                        explanationScrollPosition.y =
                            explanationScrollPositionAnimated.Evaluate(time);
                    else
                        explanationScrollPositionAnimated = (AnimationCurve)null;
                }

                Listing_Standard listing = new Listing_Standard();
                float width = position.xMax - explanationInnerRect.x;
                listing.ColumnWidth = (float)((double)width / 2.0 - 17.0);
                Rect rect3 = new Rect(0.0f, Math.Max(position.yMax, val2), width, 9999f);
                listing.Begin(rect3);
                Text.Font = GameFont.Medium;
                listing.Indent(15f);
                listing.Label("DifficultyCustomSectionLabel".Translate());
                listing.Outdent(15f);
                Text.Font = GameFont.Small;
                listing.Gap();
                if (listing.ButtonText((string)"DifficultyReset".Translate()))
                    MakeResetDifficultyFloatMenu(difficultyValues);
                float curHeight = listing.CurHeight;
                float gapHeight = outRect2.height / 2f;
                // DrawCustomLeft(listing, difficultyValues);
                listing.Gap(gapHeight);
                listing.NewColumn();
                listing.Gap(curHeight);
                // DrawCustomRight(listing, difficultyValues);
                listing.Gap(gapHeight);
                val2 = rect3.y + listing.MaxColumnHeightSeen;
                listing.End();
            }


            explanationInnerRect.height = val2;
            Widgets.EndScrollView();
            Widgets.EndGroup();
        }

        private static void MakeResetDifficultyFloatMenu(Difficulty difficultyValues)
        {
            List<FloatMenuOption> options = new List<FloatMenuOption>();
            foreach (DifficultyDef allDef in DefDatabase<DifficultyDef>.AllDefs)
            {
                DifficultyDef d = allDef;
                if (!d.isCustom)
                    options.Add(new FloatMenuOption((string)d.LabelCap, (Action)(() => difficultyValues.CopyFrom(d))));
            }

            Find.WindowStack.Add(new FloatMenu(options));
        }
    }
}