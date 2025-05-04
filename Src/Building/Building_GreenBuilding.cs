using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace GreenFight.Building
{
    public class Building_GreenBuilding : Verse.Building
    {
        
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            Log.Message("Создано");
        }

        public override void Tick()
        {
            base.Tick();
            
            Log.Message("Частый тик");
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            
            yield return new FloatMenuOption("Option 1", () =>
            {
                selPawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, 20));
            });
            // return base.GetFloatMenuOptions(selPawn);
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