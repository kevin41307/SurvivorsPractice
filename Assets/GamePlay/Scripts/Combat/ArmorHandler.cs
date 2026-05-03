using GamePlay.Scripts.Combat.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Combat
{
    /// <summary>
    /// Intent: 固定值減傷與百分比抗性分開處理，之後可從 Buff／敵人資料填入 <see cref="CombatContext.FlatArmorReduction"/>。
    /// </summary>
    public sealed class ArmorHandler : ICombatHandler
    {
        public void Handle(ref CombatContext context)
        {
            if (context.Cancelled)
            {
                return;
            }

            context.FinalDamage = Mathf.Max(0f, context.FinalDamage - context.FlatArmorReduction);
        }
    }
}
