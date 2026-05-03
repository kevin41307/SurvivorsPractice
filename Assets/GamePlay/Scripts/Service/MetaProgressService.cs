using System;
using GamePlay.Scripts.MetaProgress;
using GamePlay.Scripts.MetaProgress.Config;
using VContainer;
using System.Linq;
namespace GamePlay.Scripts.Service
{
    public sealed class MetaProgressService
    {
        [Inject] private MetaProgressRegistry registry;
        private readonly MetaProgressFileRepository repository = new();

        public int CurrentSlotId { get; private set; }
        public MetaProgressData Current { get; private set; }

        public event Action OnValueChanged;

        public MetaProgressData Load(int slotId)
        {
            CurrentSlotId = slotId;
            var data = repository.LoadOrCreate(slotId);
            
            //驗證存檔在遊戲內的定義的合法性
            if (ClampPowerUpLevelsToDefinitions(data))
            {
                repository.Save(slotId, data);
            }

            Current = data;
            OnValueChanged?.Invoke();
            return Current;
        }

        /// <summary>
        /// 將存檔中的 PowerUp 等級限制在對應 <see cref="MetaPowerUpDefinition.maxLevel"/> 內（與 <see cref="TryBuyPowerUp"/> 規則一致）。
        /// </summary>
        private bool ClampPowerUpLevelsToDefinitions(MetaProgressData data)
        {
            var modified = false;
            foreach (var id in data.PowerUpLevels.Keys.ToList())
            {
                if (!registry.TryGetPowerUp(id, out var def) || def == null)
                {
                    continue;
                }

                var cap = Math.Max(0, def.maxLevel);
                var level = data.GetPowerUpLevel(id);
                if (level > cap)
                {
                    data.TrySetPowerUpLevel(id, cap);
                    modified = true;
                }
            }

            return modified;
        }

        public void Save()
        {
            repository.Save(CurrentSlotId, Current);
        }

        public bool TryBuyPowerUp(string powerUpId, out string errorMessage, out int goldAfter, out int levelAfter)
        {
            errorMessage = string.Empty;
            goldAfter = 0;
            levelAfter = 0;

            goldAfter = Current.Gold;

            if (string.IsNullOrWhiteSpace(powerUpId))
            {
                errorMessage = "PowerUpId 無效。";
                return false;
            }

            if (!registry.TryGetPowerUp(powerUpId, out var def) || def == null)
            {
                errorMessage = $"找不到 PowerUpDefinition: '{powerUpId}'。";
                return false;
            }

            var currentLevel = Current.GetPowerUpLevel(powerUpId);
            if (currentLevel >= Math.Max(0, def.maxLevel))
            {
                levelAfter = currentLevel;
                errorMessage = "已達最大等級。";
                return false;
            }

            var cost = def.GetCostForNextLevel(currentLevel);

            if (Current.Gold < cost)
            {
                levelAfter = currentLevel;
                errorMessage = "金幣不足。";
                return false;
            }

            Current.SetGold(Current.Gold - cost);
            var newLevel = currentLevel + 1;
            Current.TrySetPowerUpLevel(powerUpId, newLevel);

            Save();
            OnValueChanged?.Invoke();

            goldAfter = Current.Gold;
            levelAfter = newLevel;
            return true;
        }
        
    }
}