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

        private Dictionary<HediffDef, int> _specialPrices = new Dictionary<HediffDef, int>()
        {
            { GreenFight.Hediff.DefOf.GreenHediffDefOf.BionicEye, 1111 }
        };
        private float _techPricePower => 3f;

        private BodyPartRecord _bodyPart;
        private int _silver => Find.CurrentMap.resourceCounter.Silver;
        

        public override Vector2 InitialSize => new Vector2(600, 400);

        public GreenWindow(Pawn pawn)
        {
            _pawn = pawn;
            resizeable = false;
            draggable = false;
            forcePause = true;
            doCloseX = true;
            RefreshProthesis();
        }

        public override void DoWindowContents(Rect inRect)
        {
            // Основной заголовок.
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0,  inRect.width, 35), $"Улучшение колонистов. Серебро: {_silver}");
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

            string buttonLabel = $"Выбрать протез ({(_selectedHediff == null ? "Не выбран" : $"{_selectedHediff.LabelCap} {GetPriceString(_selectedHediff)}")})";
            if (Widgets.ButtonText(new Rect(0, 210, 500, 25), buttonLabel))
            {

                var options = DefDatabase<HediffDef>.AllDefsListForReading
                    .Where(hf => hf.hediffClass == typeof(Hediff_Implant) || hf.hediffClass == typeof(Hediff_AddedPart))
                    .Select(hediff =>
                    {
                        return new FloatMenuOption($"{hediff.LabelCap} {GetPriceString(hediff)}", () =>
                        {
                            _selectedHediff = hediff;
                            _bodyPart = null;
                        });
                    })
                    .ToList();
                Find.WindowStack.Add(new FloatMenu(options));
            }
            
            if (Widgets.ButtonText(new Rect(0, 235, 500, 25), $"{(_bodyPart == null ? "Не выбрано" : _bodyPart.LabelCap)}"))
            {
                // Все доступные части тела
                var options = GetBodyParts()
                    .Select(part => new FloatMenuOption(part.LabelCap, () =>
                    {
                        _bodyPart = part;
                    }))
                    .DefaultIfEmpty(new FloatMenuOption("Тело", () =>
                    {
                        _bodyPart = _pawn.RaceProps.body.corePart;
                    }))
                    .ToList();
                Find.WindowStack.Add(new FloatMenu(options));
            }

            if (Widgets.ButtonText(new Rect(0, 330, 580, 25), "Купить"))
            {
                BuyProsthesis();
            }
            
            
            // Возврат глобальных значений по умолчанию.
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }
        
        // private

        private string GetPriceString(HediffDef hediff) => $"({GetPrice(hediff)} серебра)";
        
        private int GetPrice(HediffDef hediff)
        {
            if (_specialPrices.TryGetValue(hediff, out var price))
            {
                return price;
            }
            else
            {
                if (hediff.spawnThingOnRemoved != null)
                {
                    return (int)((int)hediff.spawnThingOnRemoved.techLevel * _techPricePower);
                }
                else
                {
                    return 0;
                }
            }
        }

        // Получить все части тела связанной пешки, в которые может быть установлен выбранны Heidff.
        private IEnumerable<BodyPartRecord> GetBodyParts() => from prt in _pawn.RaceProps.body.AllParts
            from rcp in DefDatabase<RecipeDef>.AllDefsListForReading
            where rcp.addsHediff != null && rcp.addsHediff == _selectedHediff &&
                  rcp.appliedOnFixedBodyParts.Contains(prt.def)
            select prt;

        private void BuyProsthesis()
        {
            if (_selectedHediff == null)
            {
                Messages.Message("Выберите протез", MessageTypeDefOf.NegativeEvent);
                return;
            }

            if (_bodyPart == null)
            {
                Messages.Message("Выберите часть тела", MessageTypeDefOf.NeutralEvent);
                return;
            }

            int price = GetPrice(_selectedHediff);
            if (!TryTakeSilverFromPlayer(price, Find.CurrentMap))
            {
                Messages.Message("Недостаточно серебра", MessageTypeDefOf.NegativeEvent);
                return;
            }

            RestorePartRecursive(_pawn, _bodyPart);
            _pawn.health.hediffSet.DirtyCache();
            _pawn.health.CheckForStateChange(null, null);
            _pawn.health.AddHediff(_selectedHediff, _bodyPart);
            Messages.Message("Протез куплен", MessageTypeDefOf.NegativeEvent);
            RefreshProthesis();
        }

        private bool TryTakeSilverFromPlayer(int count, Verse.Map map)
        {
            if (_silver >= count)
            {
                int remaining = count;
                List<Thing> silver = map.listerThings.ThingsOfDef(ThingDefOf.Silver);
                for (int i = silver.Count - 1; i >= 0; i--)
                {
                    Thing thing = silver[i];
                    int num = Mathf.Min(remaining, thing.stackCount);
                    thing.SplitOff(num).Destroy();
                    remaining -= num;
                    
                    if (remaining == 0)
                        break;
                }
                
                map.resourceCounter.UpdateResourceCounts();
                return true;
            }

            return false;
        }

        private void RestorePartRecursive(Pawn pawn, BodyPartRecord part)
        {
            List<Verse.Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int num = hediffs.Count - 1; num >= 0; num--)
            {
                Verse.Hediff hediff = hediffs[num];
                if (hediff.Part == part)
                {
                    hediffs.RemoveAt(num);
                    hediff.PostRemoved();
                }
            }
            
            for (int i = 0; i < part.parts.Count; i++)
            {
                RestorePartRecursive(pawn, part);
            }
        }

        private void RefreshProthesis()
        {
            _prosthesis = _pawn.health
                .hediffSet
                .hediffs
                .Where(x => (x is Hediff_Implant || x is Hediff_AddedPart))
                .ToList();
        }
    }
}