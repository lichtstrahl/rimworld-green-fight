using System;
using System.Collections.Generic;
using System.Linq;
using GreenFight.Job.DefOf;
using RimWorld;
using UnityEngine;
using Verse;

namespace GreenFight.Building
{
    
    /**
     * Создания контекстного меню для строения GreenBuilding.
     */
    public class Actions_GreenFactory
    {
        private static string _languageKey = "MenuOptions_GreenFactory";

        public static IEnumerable<FloatMenuOption> GetOptions(Pawn pawn, Building_GreenFactory factory)
        {
            yield return CreateDamageOption(pawn);
            yield return CreateLogOption();
            yield return CreateUploadOption(pawn, factory);
            yield return CreateGetOption(pawn, factory);
        }

        public static IEnumerable<Gizmo> GetGizmos(Texture selfIcon)
        {
            yield return new Command_Action()
            {
                defaultLabel = $"{_languageKey}_NextDayAction_Label".TranslateSimple(),
                defaultDesc = $"{_languageKey}_NextDayAction_Desc".TranslateSimple(),
                icon = selfIcon,
                action = () =>
                {
                    var ticks = Find.TickManager.TicksGame;
                    Find.TickManager.DebugSetTicksGame(ticks + 60_000);
                }
            };
        }
        
        // private
        
        // Получение пешкой урона.
        private static FloatMenuOption CreateDamageOption(Pawn pawn)
        {
            return new FloatMenuOption($"{_languageKey}_Damage".TranslateSimple(), delegate
            {
                var damage = new DamageInfo(DamageDefOf.Bite, 5);
                pawn.TakeDamage(damage);
            });
        }
        
        
        // Вывод сообщения в лог.
        private static FloatMenuOption CreateLogOption()
        {
            return new FloatMenuOption($"{_languageKey}_Log".TranslateSimple(), () =>
            {
                Log.Message("Выбрали опцию в меню");
            });
        }
        
        // Выпадающее доп. меню на загрузку любого наркотика на карте.
        private static FloatMenuOption CreateUploadOption(Pawn pawn, Building_GreenFactory factory)
        {
            Action action = () =>
            {
                var drugOptions = factory.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug)
                    .Select(drug => CreateUploadDrugOption(pawn, factory, drug))
                    .ToList();

                if (drugOptions.Count > 0)
                {
                    Find.WindowStack.Add(new FloatMenu(drugOptions));
                }
                else
                {
                    Log.Warning("На карте нет наркотиков.");
                }
            };

            return new FloatMenuOption(
                label: $"{_languageKey}_Upload".TranslateSimple(),
                action: factory.IsEmpty() ? action : null
            );
        }

        // Выдача пешке работы на погрузку наркотика.
        private static FloatMenuOption CreateUploadDrugOption(Pawn pawn, Building_GreenFactory factory, Thing drug)
        {
            return new FloatMenuOption(drug.Label, delegate
            {
                Verse.AI.Job job = new Verse.AI.Job(GreenJobDefOf.UploadItem, factory, drug)
                {
                    count = 1, 
                    playerForced = true
                };

                pawn.jobs.TryTakeOrderedJob(job);
            });
        }
        
        // Выгрузить предмет из фабрики
        private static FloatMenuOption CreateGetOption(Pawn pawn, Building_GreenFactory factory)
        {
            Action action = () =>
            {
                Verse.AI.Job job = new Verse.AI.Job(GreenJobDefOf.GetItem, factory)
                {
                    playerForced = true
                };
                pawn.jobs.TryTakeOrderedJob(job);
            };
            
            return new FloatMenuOption(
                label: $"{_languageKey}_Get".TranslateSimple(),
                action: !factory.IsEmpty() && factory.HasPower() ? action : null
            );
        }
    }
}