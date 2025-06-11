using Verse;

namespace GreenFight.Building
{
    public class Comp_GreenFactory : ThingComp
    {
        public CompProperties_GreenFactory Props => (CompProperties_GreenFactory)props;


        public override void CompTick()
        {
            base.CompTick();

            // Здесь код, который вызывается уже после основного метода (в Thing/ThingWithComps)
        }
    }
}