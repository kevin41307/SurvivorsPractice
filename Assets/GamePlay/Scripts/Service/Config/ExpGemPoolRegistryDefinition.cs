using UnityEngine;
using GamePlay.Scripts.Item.Config;

namespace GamePlay.Scripts.Service.Config
{
    [CreateAssetMenu(
        fileName = "ExpGemPoolRegistryDefinition",
        menuName = "Scriptable Objects/Service/ExpGemPoolRegistryDefinition")]
    public class ExpGemPoolRegistryDefinition : ScriptableObject
    {
        public ExperienceGemDefinition[] definitions;
    }
}

