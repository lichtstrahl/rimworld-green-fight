using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.World
{
    public class Page_GreenSelectStartingSite : Page_SelectStartingSite
    {
        public override void ExtraOnGUI()
        {
            Text.Anchor = TextAnchor.UpperCenter;
            DrawPageTitle(new Rect(0.0f, 5f, UI.screenWidth, 300f));
            Text.Anchor = TextAnchor.UpperLeft;
            
            DoButtonButtons();
            
            // Вырезанный код с указывающей стрелкой
        }

        private void DoButtonButtons()
        {
            
        }
    }
}