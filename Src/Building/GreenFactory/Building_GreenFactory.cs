using System.Collections.Generic;
using GreenFight.Condition.DefOf;
using RimWorld;
using Verse;
using UnityEngine;

namespace GreenFight.Building
{
    public class Building_GreenFactory : Verse.Building
    {
        private Comp_GreenFactory _compGreenFactory;
        private CompPowerTrader _compPowerTrader;
        private static string _languageKey = "Building_GreenFactory";

        private Thing _container;
        
        // Override
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _compGreenFactory = GetComp<Comp_GreenFactory>();
            _compPowerTrader = GetComp<CompPowerTrader>();
            Log.Message("Создано Building_GreenBuilding");
        }

        public override void Tick()
        {
            base.Tick();
            
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn) =>
            Actions_GreenFactory.GetOptions(selPawn, this);

        public override IEnumerable<Gizmo> GetGizmos() =>
            Actions_GreenFactory.GetGizmos(def.uiIcon);

        public override string GetInspectString()
        {
            return IsEmpty()
                ? $"{_languageKey}_State_Empty".TranslateSimple()
                : $"{_languageKey}_State_Full".Translate(1).ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref _container, "container");
        }

        // API

        public void Upload(Thing item)
        {
            _container = item;
        }

        public bool IsEmpty() => _container == null;

        public bool HasPower()
        {
            bool hasSolarFlare = Map.GameConditionManager.ConditionIsActive(GreenConditionDefOf.SolarFlare);
            return _compPowerTrader.PowerOn && !hasSolarFlare;
        }
        
        // private
        
        
    }
}