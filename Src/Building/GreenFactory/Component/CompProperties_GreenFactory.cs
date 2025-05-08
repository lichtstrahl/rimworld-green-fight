using Verse;

namespace GreenFight.Building
{
    public class CompProperties_GreenFactory : CompProperties
    {

        public float Damage;

        public CompProperties_GreenFactory()
        {
            compClass = typeof(Comp_GreenFactory);
        }
    }
}