using System;
using System.Linq;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.LevelUp.Ports;
using GamePlay.Scripts.Service;

namespace GamePlay.Scripts.LevelUp.Commands
{
    public sealed class AddOrUpgradeWeaponCommand : ILevelUpCommand
    {
        private readonly Build build;
        private readonly WeaponViewDefinition viewDefinition;
        private readonly WeaponFactory weaponFactory;

        public AddOrUpgradeWeaponCommand(Build build, WeaponViewDefinition viewDefinition, WeaponFactory weaponFactory)
        {
            this.build = build ?? throw new ArgumentNullException(nameof(build));
            this.viewDefinition = viewDefinition ?? throw new ArgumentNullException(nameof(viewDefinition));
            this.weaponFactory = weaponFactory ?? throw new ArgumentNullException(nameof(weaponFactory));
        }

        public void Execute()
        {
            var existing = build.Weapons.FirstOrDefault(w => w != null && w.Definition == viewDefinition);
            if (existing != null)
            {
                existing.Upgrade();
                return;
            }

            var weapon = weaponFactory.Create(viewDefinition);
            build.AddWeapon(weapon.Weapon);
        }

        public int Weight => viewDefinition.definition.Weight;
    }
}

