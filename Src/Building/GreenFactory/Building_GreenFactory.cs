using System;
using System.Collections.Generic;
using GreenFight.Condition.DefOf;
using RimWorld;
using Verse;
using UnityEngine;

namespace GreenFight.Building
{
    public class Building_GreenFactory : Verse.Building
    {
        private static string _languageKey = "Building_GreenFactory";
        private static int WorkTime = 2500 * 2;
        private static double MaxProgress = 5.0;
        
        private Comp_GreenFactory _compGreenFactory;
        private CompPowerTrader _compPowerTrader;

        private Thing _container;
        private int _workProgress = 0;
        
        // Override
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _compGreenFactory = GetComp<Comp_GreenFactory>();
            _compPowerTrader = GetComp<CompPowerTrader>();
            Log.Message("Создано Building_GreenBuilding");
        }

        public override void TickRare()
        {
            base.TickRare();

            if (HasPower() && GetCurrentProgress() < MaxProgress)
            {
                _workProgress += 2000;
            }
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn) =>
            Actions_GreenFactory.GetOptions(selPawn, this);

        public override IEnumerable<Gizmo> GetGizmos() =>
            Actions_GreenFactory.GetGizmos(def.uiIcon);

        public override string GetInspectString()
        {
            var count = 1;
            var progress = GetCurrentProgress().ToString("f2");
            
            return IsEmpty()
                ? $"{_languageKey}_State_Empty".TranslateSimple()
                : $"{_languageKey}_State_Full".Translate(count, progress, MaxProgress, GetPowerState())
                    .ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref _container, "container");
            Scribe_Values.Look(ref _workProgress, "_workStartTs");
        }

        // API

        public void GetItem()
        {
            if (_container != null)
            {
                _container.stackCount = (int)Math.Truncate(GetCurrentProgress());
                GenDrop.TryDropSpawn(_container, Position + IntVec3.South * 3, Map, ThingPlaceMode.Near, out _);
                _container = null;
            }
        }
        
        public void Upload(Thing item)
        {
            _container = item;
            _workProgress = 0;
        }

        public bool IsEmpty() => _container == null;

        public bool HasPower()
        {
            bool hasSolarFlare = Map.GameConditionManager.ConditionIsActive(GreenConditionDefOf.SolarFlare);
            return _compPowerTrader.PowerOn && !hasSolarFlare;
        }

        public bool HasGetItemAction()
        {
            bool isComplete = true;
            return !IsEmpty() && isComplete;
        }

        // private

        private double GetCurrentProgress()
        {
            double work = _workProgress * 1.0 / WorkTime;
            return Math.Min(1.0 + work, MaxProgress);
        }

        private string GetPowerState()
        {
            return HasPower()
                ? $"{_languageKey}_HasPower".TranslateSimple()
                : $"{_languageKey}_NoPower".TranslateSimple();
        }
    }
}