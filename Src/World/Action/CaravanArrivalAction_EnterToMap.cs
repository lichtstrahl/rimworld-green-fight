using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GreenFight.World.Action
{
    public class CaravanArrivalAction_EnterToMap : CaravanArrivalAction_Enter
    {
        private MapParent _map;

        public override string Label => "Войти на точку";
        public override string ReportString => "Отчет по точке";

        public CaravanArrivalAction_EnterToMap(MapParent map)
        {
            _map = map;
        }

        // Доступна ли точка.
        public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
        {
            return !(_map != null && _map.Tile != destinationTile && CanVisit(caravan, _map));
        }

        public override void Arrived(Caravan caravan)
        {
            LongEventHandler.QueueLongEvent(
                () => { EnterOnNewMap(caravan); },
                "GenerateGreenPointMap",
                doAsynchronously: false,
                null
            );
    }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_References.Look(ref _map, "_map");
        }
        
        // Проверяем возможность посетить точку. Возвращаем структуру - обертку над bool. Примитив вернуть нельзя.
        public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, MapParent map)
        {
            if (map == null || !map.Spawned)
            {
                return FloatMenuAcceptanceReport.WasRejected;
            }
            else
            {
                if (caravan.pawns.Count < 2)
                {
                    return FloatMenuAcceptanceReport.WithFailReason("Нельзя войти одному");
                }
            }

            return FloatMenuAcceptanceReport.WasAccepted;
        }

        // private
        
        private void EnterOnNewMap(Caravan caravan)
        {
            bool hasMap = _map.HasMap;
            IntVec3 size = new IntVec3(260, 1, 260);
            Map map = GetOrGenerateMapUtility.GetOrGenerateMap(_map.Tile, size, null);
            
            if (hasMap)
            {
                Log.Message("Прибытие. Есть карта.");
            }
            else
            {
                Log.Message("Прибытие. Карты ещё нет.");
            }
            
            // Можно сразу выложить инвентарь каравана. Можно дополнительно провалидировать клетки для появления.
            CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge);
        }
    }
}