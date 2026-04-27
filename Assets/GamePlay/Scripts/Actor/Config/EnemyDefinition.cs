using GamePlay.Scripts.Core;
using GamePlay.Scripts.Ports;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Scriptable Objects/EnemyDefinition")]
    public class EnemyDefinition : SerializedScriptableObject
    {
        [Required, SerializeReference] 
        IDropPolicy dropPolicy;
        
        [Required, SerializeReference] 
        IMovePolicy movePolicy;
        
    }
}