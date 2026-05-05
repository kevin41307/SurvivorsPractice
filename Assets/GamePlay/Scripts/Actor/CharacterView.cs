using System;
using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Status.Buff;
using GamePlay.Scripts.Targeting;
using R3;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class CharacterView : MonoBehaviour, ITargetable, ICombatable, IInvulnerable
    {
        [Inject]
        public Character Character { get; private set; }

        private bool isInvulnerable;
        public bool IsInvulnerable => isInvulnerable;

        private IDisposable invulnerabilityTimerDisposable;

        public void Initialize()
        {
        }

        Transform ITargetable.Transform => transform;
        Vector3 ICombatable.Position => transform.position;
        
        public void TakeDamage(float amount)
        {
            Debug.Log($"CharacterView TakeDamage: {amount}");
        }

        public void ApplyKnockback(Vector2 directionUnit, float dealtProduct)
        {
            //Ignored
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

        public void StartInvulnerability(float time)
        {
            invulnerabilityTimerDisposable?.Dispose();
            invulnerabilityTimerDisposable = null;

            if (time <= 0f)
            {
                isInvulnerable = false;
                return;
            }

            isInvulnerable = true;

            invulnerabilityTimerDisposable =
                Observable.Timer(TimeSpan.FromSeconds(time), destroyCancellationToken)
                    .Subscribe(this, (x, state) =>
                    {
                        state.isInvulnerable = false;
                    });
        }

    }
}