using GreenFight.Hediff.DefOf;
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

            if (Find.TickManager.TicksGame % 2_000 == 0)
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

        /**
         * Перебираем всех пешек на всех картах.
         * Смотрим только пешки не под крышей и это человек (не механоид).
         */
        private void Effect()
        {
            foreach (var map in AffectedMaps)
            {
                foreach (var pawn in map.mapPawns.AllPawns)
                {
                    if (!pawn.Position.Roofed(pawn.Map) && pawn.RaceProps.Humanlike)
                    {
                        // pawn.health.AddHediff()
                        HealthUtility.AdjustSeverity(pawn, GreenHediffDefOf.GreenHediff, GreenHediffDefOf.GreenHediff.initialSeverity);
                    }
                }
            }
        }
    }
}