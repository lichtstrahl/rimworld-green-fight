using RimWorld;
using Verse;

namespace GreenFight.Hediff
{
    public class GreenHediffComp : HediffComp
    {
        public GreenHediffCompProperties GreenProps => (GreenHediffCompProperties)props;

        public int DamageCount;

        public override void Notify_PawnDied(DamageInfo? dinfo, Verse.Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
        }

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
                DamageCount++;
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            
            // Для более сложных объектов.
            // Scribe_Defs.Look(); 
            
            Scribe_Values.Look(ref DamageCount, "damage-count");
        }
    }
}