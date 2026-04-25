using UnityEngine;
using GamePlay.Scripts.Actor.Config;
namespace GamePlay.Scripts.Service.Config
{
    [CreateAssetMenu(
        fileName = "EnemyPoolRegistryDefinition",
        menuName = "Scriptable Objects/EnemyPoolRegistryDefinition")]
    public class EnemyPoolRegistryDefinition : ScriptableObject
    {
        public EnemyDefinition[] definitions;
    }
}

