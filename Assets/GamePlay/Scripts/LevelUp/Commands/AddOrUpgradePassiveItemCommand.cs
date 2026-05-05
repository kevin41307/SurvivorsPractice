using System;
using System.Linq;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.LevelUp.Ports;
using GamePlay.Scripts.Service;

namespace GamePlay.Scripts.LevelUp.Commands
{
    public sealed class AddOrUpgradePassiveItemCommand : ILevelUpCommand
    {
        private readonly Build build;
        private readonly PassiveItemDefinition definition;
        private readonly PassiveItemFactory passiveItemFactory;

        public AddOrUpgradePassiveItemCommand(Build build, PassiveItemDefinition definition, PassiveItemFactory passiveItemFactory)
        {
            this.build = build ?? throw new ArgumentNullException(nameof(build));
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.passiveItemFactory = passiveItemFactory ?? throw new ArgumentNullException(nameof(passiveItemFactory));
        }

        public void Execute()
        {
            var existing = build.PassiveItems.FirstOrDefault(p => p != null && p.Definition == definition);
            if (existing != null)
            {
                existing.Upgrade();
                return;
            }

            var passive = passiveItemFactory.Create(definition);
            build.AddPassiveItem(passive);
        }


        public int Weight => definition.Weight;
    }
}

