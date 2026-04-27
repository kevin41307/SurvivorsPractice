using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.Service;
using GamePlay.Scripts.Service.Ports;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Application.DI
{
    public class Mvp1Installer : LifetimeScope
    {
        [Required, SerializeField] private InputActionAsset inputAction;

        [Required, SerializeField] private CharacterViewDefinition selectedCharacter;
        [Required, SerializeField] private EnemyViewDefinition selectedEnemy;
        [Required, SerializeField] private ExperienceGemViewDefinition selectedExperienceGem;


        protected override void Configure(IContainerBuilder builder)
        {
            //Unity Stuff
            builder.RegisterInstance(inputAction);
            
            //Service Stuff

            builder.Register<IPlayerLocator, PlayerLocatorService>(Lifetime.Singleton);
            
            builder.Register<Enemy>(Lifetime.Transient);
            builder.Register<EnemyFactory>(Lifetime.Singleton);
            builder.Register<EnemyPool>(Lifetime.Singleton);

            builder.Register<Character>(Lifetime.Transient);
            builder.Register<CharacterFactory>(Lifetime.Singleton);

            builder.Register<ExpGemFactory>(Lifetime.Singleton);
            builder.Register<ExpGemPool>(Lifetime.Singleton);
            
            //Debug Stuff
            builder.RegisterInstance(selectedCharacter);
            builder.RegisterInstance(selectedEnemy);
            builder.RegisterInstance(selectedExperienceGem);
            

            builder.RegisterEntryPoint<Mvp1Bootstrapper>();

        }
    }
}