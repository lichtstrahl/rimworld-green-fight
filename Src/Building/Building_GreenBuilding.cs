using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace GreenFight.Building
{
    public class Building_GreenBuilding : Verse.Building
    {
        private GreenBuildingComp _greenBuildingComp;
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _greenBuildingComp = GetComp<GreenBuildingComp>(); 
            Log.Message("Создано Building_GreenBuilding");
        }

        public override void Tick()
        {
            base.Tick();
            
            Log.Message("Tick");
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            
            yield return new FloatMenuOption("Option 1", () =>
            {
                
                selPawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, _greenBuildingComp.Props.Damage));
            });

            yield return createMenuOption();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            // yield return new Command_Ritual();

            yield return new Command_Action()
            {
                defaultLabel = "Default label",
                defaultDesc = "Default description",
                icon = def.uiIcon,
                action = () =>
                {
                    var ticks = Find.TickManager.TicksGame;
                    Find.TickManager.DebugSetTicksGame(ticks + 60_000);
                }
            };
        }


        private FloatMenuOption createMenuOption()
        {
            return new FloatMenuOption("Option 2", () =>
            {
                Log.Message("Выбрали Option 2");
            });
        }
    }
}