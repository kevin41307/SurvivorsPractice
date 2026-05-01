using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Service;
using VContainer;
using VContainer.Unity;

using GamePlay.Scripts.Service.Ports;
using GamePlay.Scripts.Stage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay.Scripts.Application.DI
{

    public class Mvp1Bootstrapper : IStartable, ITickable
    {
        [Inject] private readonly IObjectResolver resolver;
        
        [Inject] private readonly InputActionAsset inputActions;
        
        [Inject] private readonly IPlayerLocator playerLocator;
        
        [Inject] private readonly CharacterFactory characterFactory;
        [Inject] private readonly WeaponFactory weaponFactory;
        [Inject] private readonly TreasureChestFactory treasureChestFactory;
        [Inject] private readonly EnemyPool enemyPool;
        [Inject] private readonly ExpGemPool expGemPool;

        [Inject] private readonly MetaProgressService metaProgressService;
        [Inject] private readonly Run run;

        private readonly Mvp1SelectedViewDefinitions selectedViews;

        public Mvp1Bootstrapper(Mvp1SelectedViewDefinitions selectedViews)
        {
            this.selectedViews = selectedViews;
        }

        public void Start()
        {
            if (inputActions == null)
            {
                throw new Exception("[Mvp1Bootstrapper] InputActionAsset 未設定。");
            }

            var view = characterFactory.Create(selectedViews.SelectedCharacter);
            
            var player = new Player
            {
                CharacterView = view
            };
            playerLocator.SetPlayer(player);
            view.transform.position = new Vector3(50, 50, 0);

            var turret = enemyPool.Get(selectedViews.SelectedTurret);
            turret.transform.position = new Vector3(10, 10, 0);

            for(int i = 0; i < 100; i++)
            {
                var enemy = enemyPool.Get(selectedViews.SelectedEnemy);
                // #TODO: 半徑X內隨機位置
                var randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
                enemy.transform.position = randomPosition;

            }

            // var stage = new StageRuntime();

            // // MVP：先固定讀 slot 1，讓永久強化可在 Run 初始化時套用。
            // // 後續可改為由選單/UI 決定 slotId。
            // var meta = metaProgressService.Load(slotId: 1);
            // run.Start(meta, player, view.Character, stage);

            // weaponFactory.Create(selectedViews.SelectedWeapon);
            // treasureChestFactory.Create(selectedViews.SelectedTreasureChest);



            // expGemPool.Get(selectedViews.SelectedExperienceGem);



        }

        public void Tick()
        {
            
        }
    }
}
