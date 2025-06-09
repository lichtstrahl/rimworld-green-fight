using GreenFight.Mod;
using RimWorld;
using Verse;

namespace GreenFight.Events
{
    public class IncidentWorker_GreenEvent : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            map.weatherManager.TransitionTo(GreenMod.Settings.WeatherDef);

            IncidentParms raidParams = StorytellerUtility.DefaultParmsNow(IncidentDefOf.RaidEnemy.category, map);
            raidParams.points = GreenMod.Settings.RaidPowerRange.RandomInRange;

            IncidentDefOf.RaidEnemy.Worker.TryExecute(raidParams);
            
            return true;
        }
    }
}