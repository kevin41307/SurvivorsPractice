using GamePlay.Scripts.Service;
using GamePlay.Scripts.Service.Config;
using GamePlay.Scripts.Service.Ports;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Application.DI
{
    public class Mvp1Installer : LifetimeScope
    {
        [SerializeField] private InputActionAsset inputAction;
        
        [SerializeField] private CharacterRegistryDefinition characterRegistryDefinition;
        [SerializeField] private EnemyPoolRegistryDefinition enemyPoolRegistryDefinition;
        [SerializeField] private WeaponRegistryDefinition weaponRegistryDefinition;
        [SerializeField] private ExpGemPoolRegistryDefinition expGemPoolRegistryDefinition;
        [SerializeField] private TreasureChestRegistryDefinition treasureChestRegistryDefinition;

        [SerializeField] private CharacterDefinition selectedCharacter;

        protected override void Configure(IContainerBuilder builder)
        {
            //Unity Stuff
            builder.RegisterInstance(inputAction);
            
            //Service Stuff
            var characterRegistry = new CharacterRegistry( characterRegistryDefinition );
            var weaponRegistry = new WeaponRegistry( weaponRegistryDefinition );
            var treasureChestRegistry = new TreasureChestRegistry( treasureChestRegistryDefinition );
            
            var expGemPoolRegistry = new ExpGemPoolRegistry( expGemPoolRegistryDefinition );
            var enemyPoolRegistry = new EnemyPoolRegistry( enemyPoolRegistryDefinition );

            builder.RegisterInstance(characterRegistry);
            builder.RegisterInstance(enemyPoolRegistry);
            builder.RegisterInstance(weaponRegistry);
            builder.RegisterInstance(expGemPoolRegistry);
            builder.RegisterInstance(treasureChestRegistry);

            builder.Register<IPlayerLocator, PlayerLocatorService>(Lifetime.Singleton);
            
            //Debug Stuff
            builder.RegisterInstance(selectedCharacter);

            builder.RegisterEntryPoint<Mvp1Bootstrapper>();

        }
    }
}