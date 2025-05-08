using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Building
{
    
    /**
     * Создания контекстного меню для строения GreenBuilding.
     */
    public class Actions_GreenFactory
    {
        private static string _languageKey = "MenuOptions_GreenFactory";

        public static IEnumerable<FloatMenuOption> GetOptions(Pawn pawn)
        {
            yield return CreateDamageOption(pawn);
            yield return CreateLogOption();
        }

        public static IEnumerable<Gizmo> GetGizmos(Texture selfIcon)
        {
            yield return new Command_Action()
            {
                defaultLabel = $"{_languageKey}_NextDayAction_Label".TranslateSimple(),
                defaultDesc = $"{_languageKey}_NextDayAction_Desc".TranslateSimple(),
                icon = selfIcon,
                action = () =>
                {
                    var ticks = Find.TickManager.TicksGame;
                    Find.TickManager.DebugSetTicksGame(ticks + 60_000);
                }
            };
        }
        
        // private
        
        // Получение пешкой урона.
        private static FloatMenuOption CreateDamageOption(Pawn pawn)
        {
            return new FloatMenuOption($"{_languageKey}_Damage".TranslateSimple(), delegate
            {
                var damage = new DamageInfo(DamageDefOf.Bite, 5);
                pawn.TakeDamage(damage);
            });
        }
        
        
        // Вывод сообщения в лог.
        private static FloatMenuOption CreateLogOption()
        {
            return new FloatMenuOption($"{_languageKey}_Log".TranslateSimple(), () =>
            {
                Log.Message("Выбрали опцию в меню");
            });
        }
    }
}