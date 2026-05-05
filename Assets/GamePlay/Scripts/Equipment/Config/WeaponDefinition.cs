using GamePlay.Scripts.Core;
using GamePlay.Scripts.Loot;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Equipment.Config
{
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "Scriptable Objects/WeaponDefinition")]
    public class WeaponDefinition : SerializedScriptableObject, IWeightEntry
    {
        [MinValue(1)] public int maxLevel = 10;
        [MinValue(1)] public int rarity = 100;
        
        [MinValue(0)] public float damage;
        [MinValue(0)] public float damageBonusPerLevel;
        [MinValue(0)] public float area;
        [MinValue(0)] public float areaBonusPerLevel;
        [MinValue(0)] public float speed;
        [MinValue(0)] public float speedBonusPerLevel;
        [MinValue(0)] public float amount;
        [MinValue(0)] public float amountBonusPerLevel;
        [MinValue(0)] public float duration;
        [MinValue(0)] public float durationBonusPerLevel;
        [MinValue(0)] public float pierce;
        [MinValue(0)] public float pierceBonusPerLevel;
        [MinValue(0.01f)] public float cooldown;
        [MaxValue(0)] public float cooldownBonusPerLevel;
        [MinValue(0)] public float hitboxDelay;
        [MinValue(0)] public float knockback;
        [MinValue(0)] public int poolLimit;
        
        public float GetDamageStat(int level) => PointSlopeForm(damage, damageBonusPerLevel, level);
        public float GetCooldownStat(int level) => PointSlopeForm(cooldown, cooldownBonusPerLevel, level);
        public float GetKnockbackStat() => knockback;
        
        float PointSlopeForm(float baseValue, float a, int level)
        {
            level = Mathf.Clamp(level - 1, 0, maxLevel);
            return baseValue + a * level ;
        }

        public int Weight => rarity;
    }
    
}
