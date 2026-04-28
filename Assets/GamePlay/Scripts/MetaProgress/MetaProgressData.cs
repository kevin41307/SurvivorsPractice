using System;
using System.Collections.Generic;

namespace GamePlay.Scripts.MetaProgress
{
    /// <summary>
    /// 跨 Run 保留的永久進度（每個存檔槽 1 份）。
    /// </summary>
    public sealed class MetaProgressData
    {
        public int Gold { get; private set; }

        public IReadOnlyDictionary<string, int> PowerUpLevels => powerUpLevels;

        private readonly Dictionary<string, int> powerUpLevels = new();

        public static MetaProgressData Create()
        {
            return new MetaProgressData
            {
                Gold = 0,
            };
        }

        public static MetaProgressData FromDto(MetaProgressDataDto dto)
        {
            var p = new MetaProgressData
            {
                Gold = Math.Max(0, dto.gold),
            };

            foreach (var e in dto.powerUps)
            {
                if (string.IsNullOrWhiteSpace(e.id))
                {
                    continue;
                }

                p.powerUpLevels[e.id] = Math.Max(0, e.level);
            }

            return p;
        }

        public MetaProgressDataDto ToDto()
        {
            var dto = new MetaProgressDataDto
            {
                gold = Gold,
                powerUps = new List<MetaProgressDataDto.PowerUpEntry>(powerUpLevels.Count),
            };

            foreach (var (id, level) in powerUpLevels)
            {
                dto.powerUps.Add(new MetaProgressDataDto.PowerUpEntry { id = id, level = level });
            }

            return dto;
        }

        public int GetPowerUpLevel(string powerUpId)
        {
            if (string.IsNullOrWhiteSpace(powerUpId))
            {
                return 0;
            }

            return powerUpLevels.TryGetValue(powerUpId, out var level) ? level : 0;
        }

        public void SetGold(int gold)
        {
            Gold = Math.Max(0, gold);
        }

        public void AddGold(int delta)
        {
            SetGold(Gold + delta);
        }

        public bool TrySetPowerUpLevel(string powerUpId, int level)
        {
            if (string.IsNullOrWhiteSpace(powerUpId))
            {
                return false;
            }

            powerUpLevels[powerUpId] = Math.Max(0, level);
            return true;
        }
    }
}

