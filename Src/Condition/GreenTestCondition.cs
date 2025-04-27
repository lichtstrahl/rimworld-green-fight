using RimWorld;
using Verse;

namespace GreenFight.Condition
{
    public class GreenTestCondition : GameCondition
    {

        /**
         * Аналог конструктора. Выполняем инициализацию.
         */
        public override void Init()
        {
            base.Init();
            Log.Message($"Def ${def.defName} executed");
        }
        
        /**
         * Постоянно вызывающийся "тик" при работающем событии.
         * В одном дне 
         * Обрабатываем все задействованные карты, добавляя небольшую паузу при обработке.
         */
        public override void GameConditionTick()
        {
            base.GameConditionTick();

            if (Find.TickManager.TicksGame % 1_000 == 0)
            {
                Effect();
            }
        }

        /**
         * Для каких-то визуальных эффектов при работе события. Например - туман.
         */
        public override void GameConditionDraw(Map map)
        {
            base.GameConditionDraw(map);
        }

        public override void End()
        {
            base.End();
            Log.Message("end");
        }

        private void Effect()
        {
            foreach (var map in AffectedMaps)
            {
                foreach (var pawn in map.mapPawns.AllPawns)
                {
                    var damage = new DamageInfo(DamageDefOf.Stab, 5f);
                    pawn.TakeDamage(damage);
                }
            }
        }
    }
}