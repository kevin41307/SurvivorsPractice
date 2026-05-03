using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Status.Buff;
using GamePlay.Scripts.Targeting;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class CharacterView : MonoBehaviour, ITargetable, ICombatable
    {
        [Inject]
        public Character Character { get; private set; }

        public void Initialize()
        {
        }

        Transform ITargetable.Transform => transform;
        
        public void TakeDamage(float amount)
        {
            Debug.Log($"CharacterView TakeDamage: {amount}");
        }
        
        public void AddPassiveItem(PassiveItem item)
        {
            Character.Build.AddPassiveItem(item);
        }

        public void AddPowerUp(PowerUp item)
        {
            Character.Build.AddPowerUp(item);
        }

        public void AddWeapon(Weapon item)
        {
            Character.Build.AddWeapon(item);
        }
    }
}