using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace GreenFight.Events
{
    public class IncidentWorker_TestIndcident : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return TestEvent(parms);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            return TestEvent(parms);
        }

        private bool TestEvent(IncidentParms parms)
        {
            Verse.Map map = (Verse.Map)parms.target;
            int pawnsCount = map.mapPawns.ColonistsSpawnedCount;
            if (pawnsCount < 5)
            {
                Log.Message($"Colonist count -> {pawnsCount}");
                SendStandardLetter(def.letterLabel, def.letterText, LetterDefOf.ThreatBig, parms, null);
                return false;
            }
            else
            {
                foreach (var pawn in map.mapPawns.FreeColonists)
                {
                    Log.Message($"Pawn -> {pawn.Name.ToStringFull}");
                }

                SendStandardLetter(def.letterLabel, def.letterText, LetterDefOf.NeutralEvent, parms, null);
                return true;
            }
        }
    }
}
