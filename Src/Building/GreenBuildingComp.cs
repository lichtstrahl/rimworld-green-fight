using Verse;

namespace GreenFight.Building
{
    public class GreenBuildingComp : ThingComp
    {
        public CompProperties_GreenBuilding Props => (CompProperties_GreenBuilding)props;


        public override void CompTick()
        {
            base.CompTick();

            // Здесь код, который вызывается уже после основного метода (в Thing/ThingWithComps)
        }
    }
}