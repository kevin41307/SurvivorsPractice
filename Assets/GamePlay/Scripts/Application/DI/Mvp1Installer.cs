using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Service;
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

        [SerializeField] private CharacterViewDefinition selectedCharacter;
        [SerializeField] private EnemyViewDefinition selectedEnemy;


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
            
            //Debug Stuff
            builder.RegisterInstance(selectedCharacter);
            builder.RegisterInstance(selectedEnemy);

            builder.RegisterEntryPoint<Mvp1Bootstrapper>();

        }
    }
}