using GamePlay.Scripts.Core;
using GamePlay.Scripts.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Scriptable Objects/EnemyDefinition")]
    public class EnemyDefinition : GuidScriptableObject
    {
        public EnemyViewDefinition viewDefinition;
        
        [SerializeReference] 
        IDropPolicy dropPolicy;
        
        [SerializeReference] 
        IMovePolicy movePolicy;
        
    }
}