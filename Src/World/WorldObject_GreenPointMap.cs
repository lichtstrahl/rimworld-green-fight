using System.Collections.Generic;
using GreenFight.World.Action;
using RimWorld.Planet;
using Verse;

namespace GreenFight.World
{
    public class WorldObject_GreenPointMap : MapParent
    {
        public CaravanArrivalAction_EnterToMap enterToToMap;

        public WorldObject_GreenPointMap()
        {
            enterToToMap = new CaravanArrivalAction_EnterToMap(this);
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
        {
            return CaravanArrivalActionUtility.GetFloatMenuOptions(
                () => CaravanArrivalAction_EnterToMap.CanVisit(caravan, this),
                () => enterToToMap,
                "Выполнить вход",
                caravan,
                Tile,
                this
            );
        }

        // Удаляем карту вместе с объектов в World, если там не осталось пешек.
        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
        {
            alsoRemoveWorldObject = true;
            bool hasPwans = base.Map.mapPawns.AnyPawnBlockingMapRemoval;
            return !hasPwans;
        }
    }
}