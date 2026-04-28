using GamePlay.Scripts.Status.Ports;
using Kryz.RPG.Stats.Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.MetaProgress.Config
{
    [CreateAssetMenu(fileName = "MetaPowerUpDefinition", menuName = "Scriptable Objects/MetaPowerUpDefinition")]
    public class MetaPowerUpDefinition : ScriptableObject
    {
        [Required] public string id;

        [MinValue(0)]
        public int maxLevel = 1;

        /// <summary>
        /// 第一次購買（升到 Lv1）的花費。
        /// </summary>
        [MinValue(0)]
        public int initialCost = 0;

        /// <summary>
        /// 後續每次升級（Lv1→Lv2, Lv2→Lv3...）的花費。
        /// </summary>
        [MinValue(0)]
        public int costPerLevel = 0;

        public StatType statType;
        public StatModifierType statModifierType;

        /// <summary>
        /// 每升 1 級提供的加成（加法）。
        /// </summary>
        public float bonusPerLevel = 0f;

        public int GetCostForNextLevel(int currentLevel)
        {
            return currentLevel <= 0 ? initialCost : costPerLevel;
        }

        public float GetTotalBonus(int level)
        {
            return Mathf.Max(0, level) * bonusPerLevel;
        }
    }
}