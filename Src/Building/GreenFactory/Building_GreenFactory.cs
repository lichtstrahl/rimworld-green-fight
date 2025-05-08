using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace GreenFight.Building
{
    public class Building_GreenFactory : Verse.Building
    {
        private Comp_GreenFactory _compGreenFactory;
        private static string _languageKey = "Building_GreenFactory";

        private Thing _container;
        // private static string _languageKey = "Building_GreenFactory";
        
        // Override
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _compGreenFactory = GetComp<Comp_GreenFactory>(); 
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
            return isEmpty()
                ? $"{_languageKey}_State_Empty".TranslateSimple()
                : $"{_languageKey}_State_Full".Translate(1).ToString();
        }

        // API

        public void upload(Thing item)
        {
            _container = item;
        }

        public bool isEmpty() => _container == null;
    }
}