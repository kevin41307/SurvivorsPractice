using GamePlay.Scripts.Item.Config;
using UnityEngine;

namespace GamePlay.Scripts.Item
{
    public class PassiveItem
    {
        public PassiveItemDefinition Definition { get; } 
        public int Level { get; private set; }

        public PassiveItem(PassiveItemDefinition definition)
        {
            Definition = definition;
            Level = 1;
        }

        public void Upgrade()
        {
            if (Level >= Definition.maxLevel)
            {
                Debug.LogError($"{Definition.name} 已達最大等級 {Definition.maxLevel}");
                return;
            }
            Level++;
        }
        
        
        public float GetTotalBonus()
        {
            return Mathf.Max(0, Level) * Definition.bonusPerLevel;
        }        
        
    }
}