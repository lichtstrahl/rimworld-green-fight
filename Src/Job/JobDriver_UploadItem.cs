using System.Collections.Generic;
using System.Linq;
using GreenFight.Building;
using Verse;
using Verse.AI;

namespace GreenFight.Job
{
    
    /**
     * Загрузить предмет в строение.
     */
    public class JobDriver_UploadItem : GreenBuildingItemJobDriver
    {
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(Building, job))
            {
                return pawn.Reserve(Item, job);
            }

            return false;
        }

        /**
         * Назначение работ для пешки.
         * - Зарезервировать предмет.
         * - Подойти к предмету.
         * - Взять предмет.
         * - Подойти к постройке.
         * - Бросить предмет.
         */
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve(ItemIndex)
                .FailOnDespawnedNullOrForbidden(ItemIndex);
            yield return Toils_Goto.GotoCell(Item.Thing.Position, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(ItemIndex);
            yield return Toils_Haul.StartCarryThing(ItemIndex)
                .FailOnDespawnedNullOrForbidden(ItemIndex);
            yield return Toils_Goto.GotoCell(Building.Thing.Position, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(BuildingIndex);
            Toil upload = new Toil()
            {
                initAction = () =>
                {
                    pawn.carryTracker.TryDropCarriedThing(factory.Position, ThingPlaceMode.Near, out Thing _);
                    factory?.Upload(Item.Thing);
                    Item.Thing.DeSpawn();
                }
            };
            yield return upload;
        }
    }
    
    /**
     * Достать предмет из строения.
     */
    public class JobDriver_GetItem : GreenBuildingItemJobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return (pawn.Reserve(Building, job));
        }
        
        // Назначение работ для пешки.
        protected override IEnumerable<Toil> MakeNewToils()
        {  
            Log.Warning("Не задано никаких действий для JobDriver_GetItem");
            return Enumerable.Empty<Toil>();
        }
    }
    
    // Базовый драйвер для работы со зданием (А) и предметом для него (В).
    public abstract class GreenBuildingItemJobDriver : JobDriver
    {
        protected LocalTargetInfo Building => TargetA;
        protected TargetIndex BuildingIndex => TargetIndex.A;
        protected Building_GreenFactory factory => Building.Thing as Building_GreenFactory;
        
        protected LocalTargetInfo Item => TargetB;
        protected TargetIndex ItemIndex => TargetIndex.B;
    }
}