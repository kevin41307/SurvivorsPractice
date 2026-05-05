using System.Collections.Generic;
using GamePlay.Scripts.Status.Ports;
using Kryz.RPG.Stats.Default;

namespace GamePlay.Scripts.Status
{
    public class StatusContainer
    {

        public IReadOnlyDictionary<StatType, Stat> Stats => stats;
        
        private readonly Dictionary<StatType, Stat> stats = new();

        /// <summary>
        /// Intent:
        /// 依 <see cref="StatType"/> 取得或建立對應 Stat，避免呼叫端重複 null 檢查。
        /// </summary>
        public Stat GetOrAdd(StatType statType)
        {
            if (!stats.TryGetValue(statType, out var stat))
            {
                stat = new Stat(0f);
                stats[statType] = stat;
            }

            return stat;
        } 
        
        public void SetOrAdd(StatType statType, float initialValue)
        {
            stats[statType] = new Stat(initialValue);
        }
        
    }
}