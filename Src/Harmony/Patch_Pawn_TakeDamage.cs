using GreenFight.Thought;
using GreenFight.Trait.DefOf;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GreenFight.Harmony
{
    
    // Патч на получение урона по любой Pawn
    [HarmonyPatchCategory(nameof(HarmonyManager.Category.Thought))]
    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    public class Patch_Pawn_TakeDamage
    {

        public static void Postfix(Thing __instance, DamageInfo dinfo)
        {
            if (__instance is Pawn)
            {
                Pawn pawn = __instance as Pawn;
                TakeDamage(pawn, dinfo);
            }
            
        }

        private static void TakeDamage(Pawn pawn, DamageInfo dinfo)
        {
            if (!pawn.DeadOrDowned && pawn.RaceProps.Humanlike)
            {
                Pawn instigator = dinfo.Instigator as Pawn;
                if (instigator != null && instigator.Faction == RimWorld.Faction.OfPlayer && IsSolidarity(instigator))
                {
                    Messages.Message("EventDamageHuman".TranslateSimple(), MessageTypeDefOf.NegativeHealthEvent, false);
                    ThoughtHandler thoughts = instigator.needs.mood.thoughts;
                    thoughts.memories.TryGainMemory(GreenThoughtDefOf.GreenInflictDamageOnFriend);
                }
            }
        }

        private static bool IsSolidarity(Pawn pawn) => pawn.story.traits.HasTrait(GreenTraitDefOf.GreenSolidarity);
    }
}