using System;
using GamePlay.Scripts.MetaProgress;
using VContainer;

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
            Current = repository.LoadOrCreate(slotId);
            OnValueChanged?.Invoke();
            return Current;
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