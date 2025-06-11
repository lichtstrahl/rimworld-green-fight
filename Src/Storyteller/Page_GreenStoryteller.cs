using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Storyteller
{
    public class Page_GreenStoryteller : Page
    {
        private StorytellerDef _storyteller = GreenStorytellerDefOf.GreenStoryteller;
        private DifficultyDef _difficulty;
        private Difficulty _difficultyValues = new Difficulty();
        private Listing_Standard _selectedStorytellerInfoListing = new Listing_Standard();

        public override string PageTitle => "ChooseAIStoryteller".Translate();

        public override void PreOpen()
        {
            base.PreOpen();
            StorytellerUI.ResetStorytellerSelectionInterface();
        }

        public override void DoWindowContents(Rect rect)
        {
            DrawPageTitle(rect);
            GreenStorytellerUI.DrawStorytellerSelectionInterface(
                GetMainRect(rect),
                _storyteller,
                ref _difficulty,
                ref _difficultyValues,
                _selectedStorytellerInfoListing
            );
            DoBottomButtons(rect);
            Rect rect1 = new Rect((float)((double)rect.xMax - (double)BottomButSize.x - 200.0 - 6.0),
                rect.yMax - BottomButSize.y, 200f, BottomButSize.y);
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleRight;
            TaggedString label = "CanChangeStorytellerSettingsDuringPlay".Translate();
            Widgets.Label(rect1, label);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        protected override bool CanDoNext()
        {
            if (!base.CanDoNext())
                return false;
            if (this._difficulty == null)
            {
                if (Prefs.DevMode)
                {
                    Messages.Message("Difficulty has been automatically selected (debug mode only)",
                        MessageTypeDefOf.SilentInput, false);
                    this._difficulty = DifficultyDefOf.Rough;
                    this._difficultyValues = new Difficulty(this._difficulty);
                }
                else
                {
                    Messages.Message((string)"MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
                    return false;
                }
            }

            if (!Find.GameInitData.permadeathChosen)
            {
                if (Prefs.DevMode)
                {
                    Messages.Message("Reload anytime mode has been automatically selected (debug mode only)",
                        MessageTypeDefOf.SilentInput, false);
                    Find.GameInitData.permadeathChosen = true;
                    Find.GameInitData.permadeath = false;
                }
                else
                {
                    Messages.Message((string)"MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
                    return false;
                }
            }

            Current.Game.storyteller = new RimWorld.Storyteller(this._storyteller, this._difficulty, this._difficultyValues);
            return true;
        }
    }
}