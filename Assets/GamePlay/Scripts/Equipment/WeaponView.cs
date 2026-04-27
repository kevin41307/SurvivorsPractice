using GamePlay.Scripts.Equipment;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Equipment
{
    public class WeaponView : MonoBehaviour
    {
        [Inject]
        public Weapon Weapon { get; set; }

        public void Initialize()
        {
        }
    }
}

