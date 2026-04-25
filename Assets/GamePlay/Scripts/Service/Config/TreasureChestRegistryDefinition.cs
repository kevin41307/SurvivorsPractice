using UnityEngine;
using GamePlay.Scripts.Actor.Config;

namespace GamePlay.Scripts.Service.Config
{
    
    [CreateAssetMenu(
        fileName = "TreasureChestRegistryDefinition",
        menuName = "Scriptable Objects/TreasureChestRegistryDefinition")]
    public class TreasureChestRegistryDefinition : ScriptableObject
    {
        public TreasureChestRegistryDefinition[] definitions;
    }
}

