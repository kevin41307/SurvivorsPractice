using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyViewDefinition", menuName = "Scriptable Objects/EnemyViewDefinition")]
    public class EnemyViewDefinition : ScriptableObject
    {
        public GameObject prefab;
    }
}