using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace GreenFight.Map
{
    public class GenStep_PlayerStartSettlement : GenStep
    {
        public int size = 16;
        public bool allowGeneratingFarms = true;
        public int minBuildings = 1;
        public int minBarracks = 1;
        public RimWorld.Faction faction = RimWorld.Faction.OfPlayer;
        public int bedCount = 3;
        public int lootMarketValue = 1_000;
        

        public override int SeedPart => 398638182;

        private static List<CellRect> possibleRects = new List<CellRect>();

        public override void Generate(Verse.Map map, GenStepParams parms)
        {
            CellRect var1 = CellRect.SingleCell(map.Center + IntVec3.East * 50 + IntVec3.South * 10);
            List<CellRect> var2;
            if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out var2))
            {
                var2 = new List<CellRect>();
                MapGenerator.SetVar<List<CellRect>>("UsedRects", var2);
            }
            
            ResolveParams resolveParams = new ResolveParams();
            resolveParams.rect = this.GetOutpostRect(var1, var2, map);
            resolveParams.faction = faction;
            resolveParams.edgeDefenseWidth = 2;
            resolveParams.edgeDefenseTurretsCount = Rand.RangeInclusive(4,4);
            resolveParams.edgeDefenseMortarsCount = 2;
            resolveParams.settlementDontGeneratePawns = true;
            resolveParams.bedCount = bedCount;
            resolveParams.sitePart = parms.sitePart;
            resolveParams.attackWhenPlayerBecameEnemy = false;
            resolveParams.pawnGroupKindDef = PawnGroupKindDefOf.Settlement;
            resolveParams.settlementPawnGroupPoints = null;
            resolveParams.allowGeneratingThronerooms = true;
            resolveParams.lootMarketValue = lootMarketValue;
            resolveParams.cornerRadius = 4;
            
            BaseGen.globalSettings.map = map;
            BaseGen.globalSettings.minBuildings = minBuildings;
            BaseGen.globalSettings.minBarracks = minBarracks;
            BaseGen.globalSettings.requiredWorshippedTerminalRooms = 0;
            BaseGen.globalSettings.maxFarms = this.allowGeneratingFarms ? -1 : 0;
            BaseGen.symbolStack.Push("settlement", resolveParams);
            BaseGen.Generate();
            
            var2.Add(resolveParams.rect);
        }

        private CellRect GetOutpostRect(CellRect rectToDefend, List<CellRect> usedRects, Verse.Map map)
        {
            possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size, rectToDefend.CenterCell.z - this.size / 2,
                this.size, this.size));
            possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - this.size / 2, this.size,
                this.size));
            possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size,
                this.size, this.size));
            possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1, this.size,
                this.size));
            CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
            possibleRects.RemoveAll((Predicate<CellRect>)(x => !x.FullyContainedWithin(mapRect)));
            if (!possibleRects.Any<CellRect>())
                return rectToDefend;
            IEnumerable<CellRect> source = possibleRects.Where<CellRect>(
                (Func<CellRect, bool>)(x => !usedRects.Any<CellRect>((Predicate<CellRect>)(y => x.Overlaps(y)))));
            if (!source.Any<CellRect>())
            {
                possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size * 2,
                    rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
                possibleRects.Add(new CellRect(rectToDefend.maxX + 1 + this.size,
                    rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
                possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2,
                    rectToDefend.minZ - 1 - this.size * 2, this.size, this.size));
                possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2,
                    rectToDefend.maxZ + 1 + this.size, this.size, this.size));
            }

            return source.Any<CellRect>() ? source.RandomElement<CellRect>() : possibleRects.RandomElement<CellRect>();
        }
    }
}