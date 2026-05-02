using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Targeting;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class CharacterView : MonoBehaviour, ITargetable, ICombatable
    {
        [Inject]
        public Character Character { get; set; }

        public void Initialize()
        {
        }

        Transform ITargetable.Transform => transform;
        
        public void TakeDamage(float amount)
        {
            Debug.Log($"CharacterView TakeDamage: {amount}");
        }
    }
}