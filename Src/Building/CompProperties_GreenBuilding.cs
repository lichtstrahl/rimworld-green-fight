using Verse;

namespace GreenFight.Building
{
    public class CompProperties_GreenBuilding : CompProperties
    {

        public float Damage;

        public CompProperties_GreenBuilding()
        {
            compClass = typeof(GreenBuildingComp);
        }
    }
}