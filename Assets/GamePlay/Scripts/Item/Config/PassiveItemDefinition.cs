using GamePlay.Scripts.Status.Ports;
using Kryz.RPG.Stats.Default;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Item.Config
{
    [CreateAssetMenu(fileName = "PassiveItemDefinition", menuName = "Scriptable Objects/PassiveItemDefinition", order = 0)]
    public class PassiveItemDefinition : ScriptableObject
    {
        [MinValue(0)]
        public int maxLevel = 1;
        
        public StatType statType;
        public StatModifierType statModifierType;
        
        /// <summary>
        /// 每升 1 級提供的加成（加法）。
        /// </summary>
        public float bonusPerLevel = 0f;
        
        
    }
}