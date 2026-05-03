using GamePlay.Scripts.Combat.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Combat
{
    /// <summary>
    /// 單次傷害結算上下文；由責任鏈依序讀寫，最後由 ApplyHp 呼叫 <see cref="ICombatable.TakeDamage"/>。
    /// </summary>
    public struct CombatContext
    {
        public Transform CasterTransform;
        public ICombatable Target;
        public float RawDamage;
        public float FinalDamage;
        public bool Cancelled;
        public float FlatArmorReduction;
        public float ResistanceFraction;
    }
}
