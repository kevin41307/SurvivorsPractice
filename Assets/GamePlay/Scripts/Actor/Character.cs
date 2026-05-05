using System;
using GamePlay.Scripts.Status;

namespace GamePlay.Scripts.Actor
{
    public class Character
    {
        public Build Build { get; set; } = new();

        public StatusContainer StatusContainer { get; } = new();
        
        public int GoldCoin { get; private set; } = 0;
        public int Level { get; private set; } = 1;
        public int Experience { get; private set; } = 0;

        public void AddCoin(int amount)
        {
            GoldCoin = Math.Max(0, GoldCoin + amount);
        }

        public void SubtractCoin(int amount)
        {
            GoldCoin = Math.Max(0, GoldCoin - amount);
        }

        public void AddExperience(int amount)
        {
            Experience = Math.Max(0, Experience + amount);
        }

        
    }
}
