using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.Service;
using VContainer;
using VContainer.Unity;

using GamePlay.Scripts.Service.Ports;
using GamePlay.Scripts.Stage;
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
        
        [Inject] private readonly CharacterViewDefinition selectedCharacter;
        [Inject] private readonly EnemyViewDefinition selectedEnemy;
        [Inject] private readonly ExperienceGemViewDefinition selectedExperienceGem;
        [Inject] private readonly WeaponViewDefinition selectedWeapon;
        [Inject] private readonly TreasureChestViewDefinition selectedTreasureChest;
        

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

            var stage = new StageRuntime();

            // MVP：先固定讀 slot 1，讓永久強化可在 Run 初始化時套用。
            // 後續可改為由選單/UI 決定 slotId。
            var meta = metaProgressService.Load(slotId: 1);
            run.Start(meta, player, view.Character, stage);

            weaponFactory.Create(selectedWeapon);
            treasureChestFactory.Create(selectedTreasureChest);

            enemyPool.Get(selectedEnemy);

            expGemPool.Get(selectedExperienceGem);



        }

        public void Tick()
        {
            
        }
    }
}
