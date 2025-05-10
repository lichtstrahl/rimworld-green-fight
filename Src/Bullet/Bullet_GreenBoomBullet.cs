using RimWorld;
using Verse;

namespace GreenFight.Bullet
{
    public class Bullet_GreenBoomBullet : RimWorld.Bullet
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);

            if (hitThing != null)
            {
                GenExplosion.DoExplosion(
                    hitThing.Position,
                    hitThing.Map,
                    Rand.Range(1, 3),
                    DamageDefOf.Bomb,
                    launcher
                );
            }
        }
    }
}