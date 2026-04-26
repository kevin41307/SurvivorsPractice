using System;
using GamePlay.Scripts.Actor;
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
        private readonly CharacterDefinition selectedCharacter;

        public Mvp1Bootstrapper(
            IObjectResolver resolver, 
            IPlayerLocator playerLocator,
            InputActionAsset inputActions,
            CharacterDefinition selectedCharacter)
        {
            this.resolver = resolver;
            this.playerLocator = playerLocator;
            this.inputActions = inputActions;
            this.selectedCharacter = selectedCharacter;
        }

        public void Start()
        {
            if (inputActions == null)
            {
                throw new Exception("[Mvp1Bootstrapper] InputActionAsset 未設定。");
            }

            var go = resolver.Instantiate(selectedCharacter.viewDefinition.prefab);
           
            if (!go.TryGetComponent(out CharacterView view))
            {
                throw new Exception($"[{selectedCharacter.name}] 未找到 CharacterView 元件。");
            }

            var character = new Character();
            view.Character = character;
            
            var player = new Player
            {
                Character = character
            };
            playerLocator.SetPlayer(player);
        }

        public void Tick()
        {
            
        }
    }
}
