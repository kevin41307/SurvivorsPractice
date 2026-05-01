using UnityEngine;
using GamePlay.Scripts.Actor.Config;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    [RequireComponent(typeof(EnemyMovementController))]
    public class EnemyView : MonoBehaviour
    {
        [Inject]
        public Enemy Enemy { get; set; }

        internal int PoolKey { get; set; }
        public EnemyViewDefinition ViewDefinition { get; private set; }

        public void Initialize(int poolKey, EnemyViewDefinition viewDefinition)
        {
            PoolKey = poolKey;
            ViewDefinition = viewDefinition;
        }

        
    }
}