using GamePlay.Scripts.Combat.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Combat
{
    public sealed class KnockbackHandler : ICombatHandler
    {
        const float knockbackTakenDefault = 1f;
        
        public void Handle(ref CombatContext context)
        {
            if (context.Cancelled || context.Target == null || context.FinalDamage <= 0f)
            {
                return;
            }

            if (context.KnockbackDealt <= 0f)
            {
                return;
            }

            var dir = context.KnockbackDirection;
            if (dir.sqrMagnitude < 1e-6f)
            {
                return;
            }

            context.Target.ApplyKnockback(dir.normalized, context.KnockbackDealt * knockbackTakenDefault);
        }
    }
}
