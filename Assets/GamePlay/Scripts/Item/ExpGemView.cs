using UnityEngine;

namespace GamePlay.Scripts.Item
{
    public class ExpGemView : MonoBehaviour
    {
        internal int PoolKey { get; set; }

        public void Initialize(int poolKey)
        {
            PoolKey = poolKey;
        }
    }
}

