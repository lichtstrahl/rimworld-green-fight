using GreenFight.World;
using GreenFight.World.DefOf;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GreenFight.Events
{
    // Событие для создания объекта на карте
    public class IncidentWorker_GreenPointSpawn : IncidentWorker
    {
        
        // Находим расположение тайла на карте. Создаём там объект. Добавляем его в мир и посылаем оповещение.
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            int tile = 0;
            TileFinder.TryFindNewSiteTile(out tile);

            WorldObject_GreenPointMap greenPoint = (WorldObject_GreenPointMap)WorldObjectMaker
                .MakeWorldObject(Green_WorldObjectDefOf.GreenPoint);
            greenPoint.Tile = tile;

            Find.WorldObjects.Add(greenPoint);

            // Find.LetterStack.ReceiveLetter(
            //     def.letterLabel,
            //     def.letterText,
            //     LetterDefOf.NeutralEvent,
            //     new LookTargets(greenPoint)
            // );
            
            SendStandardLetter(def.letterLabel, def.letterText, LetterDefOf.NeutralEvent, parms, new LookTargets(greenPoint));
            
            return true;
        }
    }
}