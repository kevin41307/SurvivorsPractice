using System;
using System.Collections.Generic;

namespace GamePlay.Scripts.MetaProgress
{
    /// <summary>
    /// JsonUtility 友善的 DTO（避免 Dictionary/HashSet 直接序列化）。
    /// </summary>
    [Serializable]
    public sealed class MetaProgressDataDto
    {
        [Serializable]
        public struct PowerUpEntry
        {
            public string id;
            public int level;
        }

        public int gold;
        public List<PowerUpEntry> powerUps = new();
    }
}

