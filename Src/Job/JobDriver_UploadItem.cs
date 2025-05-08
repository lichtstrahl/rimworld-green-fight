using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace GreenFight.Job
{
    
    /**
     * Загрузить предмет в строение.
     */
    public class JobDriver_UploadItem : GreenBuildingItemJobDriver
    {

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
            yield return Toils_Haul.DropCarriedThing()
                .FailOnDespawnedNullOrForbidden(ItemIndex);
        }
    }
    
    /**
     * Достать предмет из строения.
     */
    public class JobDriver_GetItem : GreenBuildingItemJobDriver
    {
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
        
        protected LocalTargetInfo Item => TargetB;
        protected TargetIndex ItemIndex => TargetIndex.B;
        
        // Пешка резервирует постройку и предмет для неё.
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(Building, job))
            {
                return pawn.Reserve(Item, job);
            }

            return false;
        }
    }
}