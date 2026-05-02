using GamePlay.Scripts.Targeting;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Equipment
{
    public class WeaponView : MonoBehaviour
    {
        [Inject]
        public Weapon Weapon { get; set; }

        [SerializeField] private TargetSelectView targetSelectView;

        public void Initialize()
        {
        }
    }
}

