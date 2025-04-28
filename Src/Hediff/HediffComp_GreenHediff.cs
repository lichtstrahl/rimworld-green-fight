using RimWorld;
using Verse;

namespace GreenFight.Hediff
{
    public class HediffComp_GreenHediff : HediffComp
    {
        public HediffCompProperties_GreenHediff GreenProps => (HediffCompProperties_GreenHediff)props;

        /**
         * Обработка тика болезнью (конкретной его частью).
         *
         * severityAdjustment - изменение силы болезни за этот тик. Значение можно перезаписывать т.к. оно ref.
         */
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (Pawn.IsHashIntervalTick(500))
            {
                GenExplosion.DoExplosion(Pawn.Position, Pawn.Map, GreenProps.value, DamageDefOf.Bomb, null);
            }
        }
    }
}