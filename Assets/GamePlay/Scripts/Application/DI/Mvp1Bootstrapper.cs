using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Service.Config;
using VContainer;
using VContainer.Unity;
using GamePlay.Scripts.Ports;
using GamePlay.Scripts.Service.Ports;
using UnityEngine.InputSystem;

namespace GamePlay.Scripts.Application.DI
{

    public class Mvp1Bootstrapper : IStartable, ITickable
    {
        private readonly IObjectResolver resolver;
        private readonly IPlayerLocator playerLocator;
        private readonly InputActionAsset inputActions;

        public Mvp1Bootstrapper(
            IObjectResolver resolver, 
            IPlayerLocator playerLocator,
            InputActionAsset inputActions)
        {
            this.resolver = resolver;
            this.playerLocator = playerLocator;
            this.inputActions = inputActions;
        }

        public void Start()
        {
            if (inputActions == null)
            {
                throw new Exception("[Mvp1Bootstrapper] InputActionAsset 未設定。");
            }

            var character = new Character();
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
