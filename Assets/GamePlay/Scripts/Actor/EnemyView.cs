using UnityEngine;
using GamePlay.Scripts.Actor.Config;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class EnemyView : MonoBehaviour
    {
        [Inject]
        public Enemy Enemy { get; set; }

        internal int PoolKey { get; set; }

        public void Initialize(int poolKey)
        {
            PoolKey = poolKey;
        }

        
    }
}