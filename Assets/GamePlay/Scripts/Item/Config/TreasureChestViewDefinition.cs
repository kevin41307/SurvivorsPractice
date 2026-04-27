using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Item.Config
{
    [CreateAssetMenu(fileName = "TreasureChestViewDefinition", menuName = "Scriptable Objects/TreasureChestViewDefinition")]
    public class TreasureChestViewDefinition : ScriptableObject
    {
        [Required] public GameObject prefab;
    }
}

