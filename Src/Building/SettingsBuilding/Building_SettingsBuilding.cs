using System.Collections.Generic;
using System.Linq;
using GreenFight.Mod;
using GreenFight.Mod.Extension;
using RimWorld;
using Verse;

namespace GreenFight.Building.SettingsBuilding
{
    public class Building_SettingsBuilding : Verse.Building
    {
        private DefModExtension_BuildingExtension settings;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            settings = def.GetModExtension<DefModExtension_BuildingExtension>();
        }

        public override void Tick()
        {
            base.Tick();

            if (Find.TickManager.TicksGame % settings.foodSatisfyingPower == 0)
            {
                CellRect cellRect = this.OccupiedRect().ExpandedBy(5);
                foreach (IntVec3 cell in cellRect)
                {
                    IEnumerable<Thing> pawns = cell.GetThingList(Map)
                        .Where(x => x is Pawn);

                    foreach (Thing thing in pawns)
                    {
                        Pawn pawn = thing as Pawn;
                        Need_Food needFood = pawn.needs.TryGetNeed<Need_Food>();

                        if (needFood != null)
                        {
                            needFood.CurLevel += 0.02f;
                        }
                    }
                }
            }
        }
    }
}