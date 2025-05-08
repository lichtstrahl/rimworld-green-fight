using RimWorld;
using Verse;

namespace GreenFight.Building
{
    
    /**
     * Создания контекстного меню для строения GreenBuilding.
     */
    public class MenuOptions_GreenBuilding
    {
        private static string _languageKey = "MenuOptions_GreenBuilding";

        // Получение пешкой урона.
        public static FloatMenuOption CreateDamageOption(Pawn pawn)
        {
            return new FloatMenuOption($"{_languageKey}_Damage".TranslateSimple(), delegate
            {
                var damage = new DamageInfo(DamageDefOf.Bite, 5);
                pawn.TakeDamage(damage);
            });
        }
        
        
        // Вывод сообщения в лог.
        public static FloatMenuOption CreateLogOption()
        {
            return new FloatMenuOption($"{_languageKey}_Log".TranslateSimple(), () =>
            {
                Log.Message("Выбрали опцию в меню");
            });
        }
    }
}