using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyViewDefinition", menuName = "Scriptable Objects/EnemyViewDefinition")]
    public class EnemyViewDefinition : SerializedScriptableObject
    {
        public EnemyDefinition definition;
        public GameObject prefab;
    }
}