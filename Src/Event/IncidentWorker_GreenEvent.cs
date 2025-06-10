using GreenFight.Mod;
using GreenFight.Mod.Extension;
using RimWorld;
using Verse;

namespace GreenFight.Events
{
    public class IncidentWorker_GreenEvent : IncidentWorker
    {
        private DefModExtension_EventExtension settings => def.GetModExtension<DefModExtension_EventExtension>();
        
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            map.weatherManager.TransitionTo(settings.WeatherDef);

            IncidentParms raidParams = StorytellerUtility.DefaultParmsNow(IncidentDefOf.RaidEnemy.category, map);
            raidParams.points = settings.RaidPowerRange.RandomInRange;

            IncidentDefOf.RaidEnemy.Worker.TryExecute(raidParams);
            
            return true;
        }
    }
}