using System.Collections.Generic;
using System.Linq;
using GreenFight.Building;
using RimWorld;
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
            yield return Toils_General.Wait(150)
                .WithProgressBarToilDelay(BuildingIndex)
                .FailOnDespawnedNullOrForbidden(BuildingIndex);
            
            Toil upload = new Toil()
            {
                initAction = () =>
                {
                    Thing targetItem;
                    pawn.carryTracker.TryDropCarriedThing(factory.Position, ThingPlaceMode.Near, out targetItem);
                    factory?.Upload(targetItem);
                    targetItem.DeSpawn();
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
            yield return Toils_Reserve.Reserve(BuildingIndex)
                .FailOnDespawnedNullOrForbidden(BuildingIndex);

            yield return Toils_Goto.GotoCell(Building.Cell, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(BuildingIndex);

            yield return Toils_General.Wait(150)
                .WithProgressBarToilDelay(BuildingIndex)
                .FailOnDespawnedNullOrForbidden(BuildingIndex);

            yield return new Toil
            {
                initAction = () =>
                {
                    factory?.GetItem();
                }
            };
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