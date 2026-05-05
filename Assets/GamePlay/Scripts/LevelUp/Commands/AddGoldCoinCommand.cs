using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.LevelUp.Ports;

namespace GamePlay.Scripts.LevelUp.Commands
{
    // 增加角色的金幣數量
    public class AddGoldCoinCommand : ILevelUpCommand
    {
        private readonly Character character;
        private readonly int delta;

        public AddGoldCoinCommand(Character character, int delta)
        {
            this.character = character ?? throw new ArgumentNullException(nameof(character));
            this.delta = delta;
        }

        public int Weight => 999;

        public void Execute()
        {
            character.AddCoin(delta);
        }
    }
}