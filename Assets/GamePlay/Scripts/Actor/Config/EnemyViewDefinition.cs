using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyViewDefinition", menuName = "Scriptable Objects/EnemyViewDefinition")]
    public class EnemyViewDefinition : SerializedScriptableObject
    {
        [Required] public GameObject prefab;
        public EnemyDefinition definition;
    }
}