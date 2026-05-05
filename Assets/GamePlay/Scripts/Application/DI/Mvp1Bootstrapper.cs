using System;
using System.Linq;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.LevelUp;
using GamePlay.Scripts.Service;
using VContainer;
using VContainer.Unity;

using GamePlay.Scripts.Service.Ports;
using GamePlay.Scripts.Stage;
using GamePlay.Scripts.Status.Buff;
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
        [Inject] private MetaProgressRegistry registry;
        [Inject] private readonly Run run;
        [Inject] private readonly LevelUpService levelUpService;

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

            var characterView = characterFactory.Create(selectedViews.SelectedCharacter);
            
            var player = new Player
            {
                CharacterView = characterView
            };
            playerLocator.SetPlayer(player);
            characterView.transform.position = new Vector3(15, 15, 0);

            var turret = enemyPool.Get(selectedViews.SelectedTurret);
            turret.transform.position = new Vector3(10, 10, 0);

            for(int i = 0; i < 50; i++)
            {
                var enemy = enemyPool.Get(selectedViews.SelectedEnemy);
                // #TODO: 半徑X內隨機位置
                var randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
                enemy.transform.position = randomPosition;

            }

            var weaponView = weaponFactory.Create(selectedViews.SelectedWeapon, characterView.transform);
            characterView.AddWeapon(weaponView.Weapon);

            var stage = new StageRuntime();

            // MVP：先固定讀 slot 999，讓永久強化可在 Run 初始化時套用。
            // 後續可改為由選單/UI 決定 slotId。
            var meta = metaProgressService.Load(slotId: 999);
            run.Start(meta, player, stage);

            // 簡單測試LevelUpService
            try
            {
                var character = characterView.Character;
                var beforeGold = character.GoldCoin;
                var beforeWeapons = character.Build.Weapons.Count;
                var beforePassives = character.Build.PassiveItems.Count;

                var options = levelUpService.RollOptions(character, optionCount: 3);
                Debug.Log($"[LevelUpServiceTest] Rolled {options.Count} options: {string.Join(", ", options.Select(o => o.GetType().Name))}");

                if (options.Count > 0)
                {
                    options[0].Execute();
                }

                Debug.Log(
                    $"[LevelUpServiceTest] After Execute: Gold {beforeGold}->{character.GoldCoin}, " +
                    $"Weapons {beforeWeapons}->{character.Build.Weapons.Count}, " +
                    $"Passives {beforePassives}->{character.Build.PassiveItems.Count}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LevelUpServiceTest] Failed: {ex}");
            }

            // treasureChestFactory.Create(selectedViews.SelectedTreasureChest);
            // expGemPool.Get(selectedViews.SelectedExperienceGem);





        }

        public void Tick()
        {
            
        }
    }
}
