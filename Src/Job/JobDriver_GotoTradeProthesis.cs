using System.Collections.Generic;
using GreenFight.Building;
using GreenFight.Job.DefOf;
using Verse;
using Verse.AI;

namespace GreenFight.Job
{
    public class JobDriver_GotoTradeProthesis : JobDriver
    {
        public static void TakeJob(Pawn pawn, Building_GreenFactory factory)
        {
            Verse.AI.Job job = new Verse.AI.Job(GreenJobDefOf.TradeProthesis, factory)
            {
                playerForced = true
            };
            pawn.jobs.TryTakeOrderedJob(job);
        }
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        /**
         * - Резервируем здание (A)
         * - Идём к зданию. Ошибка если нельзя его зарезервировать или кто-то другой идет к зданию.
         * - Как только пешка подошла - открываем меню выбора.
         */
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell)
                .FailOnDespawnedNullOrForbidden(TargetIndex.A)
                .FailOnSomeonePhysicallyInteracting(TargetIndex.A);
            yield return new Toil
            {
                initAction = () =>
                {
                    var factory = (Building_GreenFactory)TargetA;
                    factory.OpenTradeWindow(pawn);
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}