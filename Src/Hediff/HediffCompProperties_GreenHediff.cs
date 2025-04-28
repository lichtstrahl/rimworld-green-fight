using Verse;

namespace GreenFight.Hediff
{
    public class HediffCompProperties_GreenHediff : HediffCompProperties
    {
        public float value;
        
        public HediffCompProperties_GreenHediff()
        {
            compClass = typeof(HediffComp_GreenHediff);
        }
    }
}