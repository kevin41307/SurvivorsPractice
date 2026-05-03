using GamePlay.Scripts.Combat.Ports;

namespace GamePlay.Scripts.Combat
{
    /// <summary>
    /// Intent: 唯一呼叫 <see cref="ICombatable.TakeDamage"/> 的出口，避免繞過管線直接扣血。
    /// </summary>
    public sealed class ApplyHpHandler : ICombatHandler
    {
        public void Handle(ref CombatContext context)
        {
            if (context.Cancelled || context.Target == null || context.FinalDamage <= 0f)
            {
                return;
            }

            context.Target.TakeDamage(context.FinalDamage);
        }
    }
}
