using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Item.Config
{
    [CreateAssetMenu(fileName = "ExperienceGemViewDefinition", menuName = "Scriptable Objects/ExperienceGemViewDefinition")]
    public class ExperienceGemViewDefinition : SerializedScriptableObject
    {
        [Required] public GameObject prefab;
    }
}

