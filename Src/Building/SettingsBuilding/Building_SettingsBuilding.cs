using System.Collections.Generic;
using System.Linq;
using GreenFight.Mod;
using RimWorld;
using Verse;

namespace GreenFight.Building.SettingsBuilding
{
    public class Building_SettingsBuilding : Verse.Building
    {
        public override void Tick()
        {
            base.Tick();

            if (Find.TickManager.TicksGame % GreenMod.Settings.foodSatisfyingPower == 0)
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
                            needFood.CurLevel += 0.05f;
                        }
                    }
                }
            }
        }
    }
}