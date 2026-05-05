using System.Collections.Generic;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Combat;
using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.MetaProgress;
using GamePlay.Scripts.MetaProgress.Config;
using SpatialHash2D;
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

        [Required, SerializeField] private Mvp1SelectedViewDefinitionRefs selectedViewDefinitions;

        [SerializeField] private List<MetaPowerUpDefinition> metaPowerUps = new();

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

            builder.Register<Weapon>(Lifetime.Transient);
            builder.Register<WeaponFactory>(Lifetime.Singleton);

            builder.Register<CombatPipeline>(_ =>
                    new CombatPipeline(new List<ICombatHandler>
                    {
                        new InvulnerabilityHandler(),
                        new ArmorHandler(),
                        new ResistanceHandler(),
                        new KnockbackHandler(),
                        new ApplyHpHandler(),
                    }),
                Lifetime.Singleton);

            builder.Register<TreasureChest>(Lifetime.Transient);
            builder.Register<TreasureChestFactory>(Lifetime.Singleton);

            builder.Register<ExpGemFactory>(Lifetime.Singleton);
            builder.Register<ExpGemPool>(Lifetime.Singleton);

            builder.Register<MetaProgressRegistry>(Lifetime.Singleton)
                .WithParameter<IEnumerable<MetaPowerUpDefinition>>(metaPowerUps);
            
            // MetaProgress
            builder.Register<MetaProgressService>(Lifetime.Singleton);
            
            //Debug Stuff
            builder.Register<Run>(Lifetime.Scoped);
            builder.RegisterInstance(selectedViewDefinitions.ToDefinitions());
            
            // 明確用無參建構子：避免 VContainer 選到 SpatialHashWorld(float) 而要求註冊 System.Single
            builder.Register<SpatialHashWorld>(_ => new SpatialHashWorld(), Lifetime.Singleton);
            builder.RegisterEntryPoint<SpatialHashWorldTickEntryPoint>(Lifetime.Singleton);

            builder.RegisterEntryPoint<Mvp1Bootstrapper>();

        }
    }
}