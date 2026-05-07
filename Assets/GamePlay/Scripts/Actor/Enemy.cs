using System;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Status;
using GamePlay.Scripts.Status.Ports;

namespace GamePlay.Scripts.Actor
{
    public class Enemy
    {
        public StatusContainer StatusContainer { get; } = new();
        public float CurrentHealthPoint { get; private set; }
        public EnemyDefinition  definition { get; private set; }
        public int Level { get; private set; } = 1;

        public void Initialize(EnemyDefinition definition, int level)
        {
            this.definition = definition;
            this.Level = level;
            GetStatsByLevelFromDefinition();
        }
        
        void GetStatsByLevelFromDefinition()
        {
            StatusContainer.SetMaxHealthPoints(definition.GetHealthPointStat(Level));
            CurrentHealthPoint = StatusContainer.GetMaxHealthPoints();
        }       

        public void TakeDamage(int amount)
        {
            CurrentHealthPoint -= amount;
            if (CurrentHealthPoint <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            throw new NotImplementedException();
        }
    }
}