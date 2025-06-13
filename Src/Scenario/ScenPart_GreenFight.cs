using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GreenFight.Scenario
{
    // Создаётся при подгрузке XML определений. Здесь ещё нет доступа к полноценным Def.
    public class ScenPart_GreenFight : ScenPart
    {
        private const string _TargetDefName = "ConstructCompanyRough";
        private const int _TargetStartSettlementTileId = 12644;
        
        
        public override void PostWorldGenerate()
        {
            FactionDef constructCompanyRough = DefDatabase<FactionDef>.AllDefs
                .FirstOrDefault(f => f.defName == _TargetDefName);

            RemoveExistingSettlements(constructCompanyRough);
            CreateInitSettlement(constructCompanyRough);
        }

        private void RemoveExistingSettlements(FactionDef faction)
        {
            var settlements = Current.CreatingWorld.worldObjects.Settlements
                .Where(o => o.Faction.def == faction)
                .ToList();
            
            foreach (var settlement in settlements)
            {
                Current.CreatingWorld.worldObjects.Remove(settlement);
            }
        }

        private void CreateInitSettlement(FactionDef factionDef)
        {
            Settlement office = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
            RimWorld.Faction faction = Find.FactionManager.AllFactionsListForReading
                .FirstOrDefault(f => f.def == factionDef);
            office.SetFaction(faction);
            office.Tile = _TargetStartSettlementTileId;

            Current.CreatingWorld.worldObjects.Add(office);
        }
    }
}