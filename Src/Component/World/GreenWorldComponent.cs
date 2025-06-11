using RimWorld.Planet;
using Verse;

namespace GreenFight.Component.World
{
    public class GreenWorldComponent : WorldComponent
    {

        public GreenWorldComponent(RimWorld.Planet.World world) : base(world)
        {
            Log.Message("Worl создан");
        }
        
        public override void WorldComponentUpdate()
        {
            base.WorldComponentUpdate();
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }

        // После загрузки игры.
        public override void FinalizeInit()
        {
            base.FinalizeInit();
            Log.Message("World окончательно создан.");
        }
    }
}