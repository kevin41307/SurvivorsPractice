using GamePlay.Scripts.Actor;
using GamePlay.Scripts.MetaProgress;
using GamePlay.Scripts.Service;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using VContainer;
using GamePlay.Scripts.Stage;


namespace GamePlay.Scripts.Application
{
    public sealed class Run
    {
        [Inject] private MetaProgressRegistry registry;
        
        private StageRuntime stage;
        private Player player;
        
        public void Start(MetaProgressData metaProgress, Player player, Character character, StageRuntime stage)
        {
            ApplyMetaProgress(metaProgress, character);
        }

        private void ApplyMetaProgress(MetaProgressData metaProgress, Character character)
        {
            player.MetaMoney = metaProgress.Gold;
            
            foreach (var (id, level) in metaProgress.PowerUpLevels)
            {
                if (level <= 0)
                {
                    continue;
                }

                if (!registry.TryGetPowerUp(id, out var def) || def == null)
                {
                    continue;
                }

                float bonus = def.GetTotalBonus(level);
                if (bonus == 0f)
                {
                    return;
                }

                var stat = character.StatusContainer.GetOrAdd(def.statType);
                stat.AddModifier(new StatModifier<StatModifierData>(
                    bonus,
                    new StatModifierData(def.statModifierType, this)));                
                
            }
        }

        
    }
}