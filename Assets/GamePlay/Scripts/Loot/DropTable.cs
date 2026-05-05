using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.Core.Ports;

namespace GamePlay.Scripts.Loot
{
    public interface IWeightEntry
    {
        int Weight { get; }
    }
    
    public static class DropTableExtensions
    {

        public static T RollDrops<T>(this List<T> pool, IRandom random) where T : IWeightEntry
        {
            int totalWeight = pool.Sum(e => e.Weight);
            int roll = random.NextInt(0, totalWeight);
            int sum = 0;

            foreach (var entry in pool)
            {
                sum += entry.Weight;
                if (roll < sum)
                    return entry;
            }

            return default;
        }
    }
}
