using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Item
{
    public class TreasureChestView : MonoBehaviour
    {
        [Inject]
        public TreasureChest TreasureChest { get; set; }

        public void Initialize()
        {
        }
    }
}

