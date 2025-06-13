using System.Linq;
using GreenFight.Map;
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

        // MapGeneratorDefOf.Base_Player Используется для генерации
        public override void PreMapGenerate()
        {
            base.PreMapGenerate();
            Log.Message("GreenFight. Перед генерацией карты");
        }

        // 
        public override void GenerateIntoMap(Verse.Map map)
        {
            base.GenerateIntoMap(map);
            Log.Message("Доп. генерация");

            GenStep_PlayerStartSettlement outpost = new GenStep_PlayerStartSettlement()
            {
                minBuildings = 3,
                minBarracks = 2,
                size = 64
            };
            
            GenStepParams outpostParams = new GenStepParams
            {
                sitePart = new SitePart
                {
                    expectedEnemyCount = 5,
                    parms = new SitePartParams
                    {
                        threatPoints = 1,
                        randomValue = 10,
                        lootMarketValue = 5_000
                    }
                }
            };

            map.info.parent.SetFaction(RimWorld.Faction.OfEntities);
            outpost.Generate(map, outpostParams);
            map.info.parent.SetFaction(RimWorld.Faction.OfPlayer);
        }

        // private

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
            office.Name = NameGenerator.GenerateName(factionDef.settlementNameMaker);

            Current.CreatingWorld.worldObjects.Add(office);
        }
    }
}