using GamePlay.Scripts.Actor;
using GamePlay.Scripts.MetaProgress;
using GamePlay.Scripts.Service;
using System;
using GamePlay.Scripts.MetaProgress.Config;
using VContainer;
using GamePlay.Scripts.Stage;
using GamePlay.Scripts.Status.Buff;
using UnityEngine;


namespace GamePlay.Scripts.Application
{
    public sealed class Run
    {
        readonly object metaProgressModifierSourceKey = new();
        
        [Inject] private MetaProgressRegistry registry;
        
        private StageRuntime stage;
        private Player player;
        
        public void Start(MetaProgressData metaProgress, Player player, StageRuntime stage)
        {
            this.player = player ?? throw new ArgumentNullException(nameof(player));

            if (player.CharacterView == null)
            {
                throw new NullReferenceException("player.CharacterView is null");
            }
            
            if (player.CharacterView.Character == null)
            {
                throw  new NullReferenceException("player.CharacterView.Character is null");
            }

            ApplyMetaProgress(metaProgress, player.CharacterView.Character);
        }

        private void ApplyMetaProgress(MetaProgressData metaProgress, Character character)
        {
            player.MetaMoney = metaProgress.Gold;
            
            Debug.Log("metaProgress.PowerUpLevels: " + metaProgress.PowerUpLevels.Count);
            foreach (var (id, level) in metaProgress.PowerUpLevels)
            {
                if (level <= 0)
                {
                    Debug.LogError($"PowerUp {id} 等級為 {level}，跳過");
                    continue;
                }

                if (!registry.TryGetPowerUp(id, out MetaPowerUpDefinition def))
                {
                    Debug.LogError($"PowerUp {id} 定義不存在，跳過");
                    continue;
                }
                
                character.Build.AddPowerUp(new PowerUp(def, level));

                // float bonus = def.GetTotalBonus(level);
                // if (bonus == 0f)
                // {
                //     continue;
                // }
                //
                // var stat = character.StatusContainer.GetOrAdd(def.statType);
                // stat.AddModifier(new StatModifier<StatModifierData>(
                //     bonus,
                //     new StatModifierData(def.statModifierType, metaProgressModifierSourceKey)));


            }
        }

        
    }
}