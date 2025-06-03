using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;

namespace GreenFight.Window
{
    public class GreenWindow : Verse.Window
    {
        private Pawn _pawn;
        private Vector2 _scroll = Vector2.zero;
        private List<Verse.Hediff> _prosthesis;
        private HediffDef _selectedHediff;

        public override Vector2 InitialSize => new Vector2(600, 400);

        public GreenWindow(Pawn pawn)
        {
            _pawn = pawn;
            resizeable = false;
            draggable = false;
            forcePause = true;
            doCloseX = true;
            _prosthesis = pawn.health
                .hediffSet
                .hediffs
                .Where(x => (x is Hediff_Implant || x is Hediff_AddedPart))
                .ToList();
        }

        public override void DoWindowContents(Rect inRect)
        {
            // Основной заголовок.
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0,  inRect.width, 35), "Улучшение колонистов");
            Text.Font = GameFont.Small;

            // Портрет пешки.
            Text.Anchor = TextAnchor.UpperLeft;
            Rect pawnCardRect = new Rect(0, 35, 100, 140);
            GUI.DrawTexture(pawnCardRect, PortraitsCache.Get(_pawn, new Vector2(100, 140), Rot4.South));
            Rect pawnNameRect = new Rect(pawnCardRect.x, pawnCardRect.y + pawnCardRect.height + 10, 130, 25);
            Widgets.Label(pawnNameRect, _pawn.Name.ToStringFull);

            // Заголовок списка
            Widgets.Label(new Rect(140, 35, 440, 25), "Установленные протезы");
            Widgets.DrawLineVertical(135, 35, 200);

            // Список
            Rect scrollViewRect = new Rect(140, 65, 420, 80);
            Rect scrollRect = new Rect(0, 0, scrollViewRect.x, _prosthesis.Count * 26);
            int scrollYOffset = 10;
            Widgets.BeginScrollView(scrollViewRect, ref _scroll, scrollRect);
            foreach (var hediff in _prosthesis)
            {
                Widgets.Label(new Rect(0, scrollYOffset, 400, 25), $"{hediff.LabelCap} ({hediff.Part.LabelCap})");
                scrollYOffset += 26;
            }
            Widgets.EndScrollView();

            string buttonLabel = $"Выбрать протез ({(_selectedHediff == null ? "Не выбран" : _selectedHediff.LabelCap.RawText)})";
            if (Widgets.ButtonText(new Rect(0, 210, 500, 25), buttonLabel))
            {

                var options = DefDatabase<HediffDef>.AllDefsListForReading
                    .Where(hf => hf.hediffClass == typeof(Hediff_Implant) || hf.hediffClass == typeof(Hediff_AddedPart))
                    .Select(hediff =>
                    {
                        return new FloatMenuOption(hediff.LabelCap, () => { _selectedHediff = hediff; });
                    })
                    .ToList();
                Find.WindowStack.Add(new FloatMenu(options));
            }
            // Возврат глобальных значений по умолчанию.
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }
    }
}