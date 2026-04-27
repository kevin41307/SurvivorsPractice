using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.Service;
using VContainer;
using VContainer.Unity;
using GamePlay.Scripts.Ports;
using GamePlay.Scripts.Service.Ports;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay.Scripts.Application.DI
{

    public class Mvp1Bootstrapper : IStartable, ITickable
    {
        [Inject] private readonly IObjectResolver resolver;
        [Inject] private readonly IPlayerLocator playerLocator;
        [Inject] private readonly InputActionAsset inputActions;
        [Inject] private readonly CharacterFactory characterFactory;
        [Inject] private readonly EnemyPool enemyPool;
        [Inject] private readonly ExpGemPool expGemPool;
        [Inject] private readonly CharacterViewDefinition selectedCharacter;
        [Inject] private readonly EnemyViewDefinition selectedEnemy;

        public void Start()
        {
            if (inputActions == null)
            {
                throw new Exception("[Mvp1Bootstrapper] InputActionAsset 未設定。");
            }

            var view = characterFactory.Create(selectedCharacter);
            
            var player = new Player
            {
                CharacterView = view
            };
            playerLocator.SetPlayer(player);

            var enemyView = enemyPool.Get(selectedEnemy);

            ExperienceGemViewDefinition experienceGemDef = null;
            try
            {
                experienceGemDef = resolver.Resolve<ExperienceGemViewDefinition>();
            }
            catch
            {
                // Ignore: no exp gem definition bound in container.
            }

            if (experienceGemDef != null)
            {
                expGemPool.Get(experienceGemDef);
            }

        }

        public void Tick()
        {
            
        }
    }
}
