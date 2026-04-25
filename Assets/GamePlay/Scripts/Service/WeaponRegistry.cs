using GamePlay.Scripts.Service.Config;

namespace GamePlay.Scripts.Service
{
    public class WeaponRegistry
    {
        private readonly WeaponRegistryDefinition weaponRegistryDefinition;

        public WeaponRegistry(WeaponRegistryDefinition weaponRegistryDefinition)
        {
            this.weaponRegistryDefinition = weaponRegistryDefinition;
        }
    }
}