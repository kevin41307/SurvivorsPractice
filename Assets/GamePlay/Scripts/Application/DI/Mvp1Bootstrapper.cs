using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Service;
using GamePlay.Scripts.Service.Config;
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
        private readonly IObjectResolver resolver;
        private readonly IPlayerLocator playerLocator;
        private readonly InputActionAsset inputActions;
        private readonly CharacterViewDefinition selectedCharacter;
        private readonly EnemyViewDefinition selectedEnemy;
        private readonly CharacterFactory characterFactory;
        private readonly EnemyPool enemyPool;

        public Mvp1Bootstrapper(
            IObjectResolver resolver, 
            IPlayerLocator playerLocator,
            InputActionAsset inputActions,
            CharacterViewDefinition selectedCharacter, 
            EnemyViewDefinition selectedEnemy,
            CharacterFactory characterFactory,
            EnemyPool enemyPool)
        {
            this.resolver = resolver;
            this.playerLocator = playerLocator;
            this.inputActions = inputActions;
            this.selectedCharacter = selectedCharacter;
            this.selectedEnemy = selectedEnemy;
            this.characterFactory = characterFactory;
            this.enemyPool = enemyPool;
        }

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

        }

        public void Tick()
        {
            
        }
    }
}
