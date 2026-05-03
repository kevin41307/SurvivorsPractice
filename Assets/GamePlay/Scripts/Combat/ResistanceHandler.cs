using GamePlay.Scripts.Combat.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Combat
{
    /// <summary>
    /// Intent: 在固定護甲之後套用比例抗性，與企劃常見「先甲後抗」順序一致。
    /// </summary>
    public sealed class ResistanceHandler : ICombatHandler
    {
        public void Handle(ref CombatContext context)
        {
            if (context.Cancelled)
            {
                return;
            }

            var factor = Mathf.Clamp01(1f - Mathf.Clamp01(context.ResistanceFraction));
            context.FinalDamage *= factor;
        }
    }
}
