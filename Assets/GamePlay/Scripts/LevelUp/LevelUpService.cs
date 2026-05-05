using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Core.Ports;
using GamePlay.Scripts.Item.Config;
using GamePlay.Scripts.LevelUp.Commands;
using GamePlay.Scripts.LevelUp.Ports;
using GamePlay.Scripts.Loot;
using GamePlay.Scripts.Service;
using UnityEngine;

namespace GamePlay.Scripts.LevelUp
{
    /// <summary>
    /// LevelUpService（服務）：從 Registry 總表產生升級選項，並套用過濾規則與加權抽取。
    /// 規則：
    /// - 物品已滿級：移出池
    /// - 欄位已滿且屬於「新物品」：移出池（武器/被動各自上限）
    /// - 已持有但未滿等：仍可出現（屬於 Upgrade 選項）
    /// - 輸出選項附帶 Command，執行時建立/升級並加入 Build
    /// </summary>
    public sealed class LevelUpService
    {
        public const int MaxWeapons = 6;
        public const int MaxPassiveItems = 6;

        private readonly WeaponRegistry weaponRegistry;
        private readonly PassiveItemRegistry passiveItemRegistry;
        private readonly WeaponFactory weaponFactory;
        private readonly PassiveItemFactory passiveItemFactory;
        private readonly IRandomProvider randomProvider;

        public LevelUpService(
            WeaponRegistry weaponRegistry,
            PassiveItemRegistry passiveItemRegistry,
            WeaponFactory weaponFactory,
            PassiveItemFactory passiveItemFactory,
            IRandomProvider randomProvider)
        {
            this.weaponRegistry = weaponRegistry ?? throw new ArgumentNullException(nameof(weaponRegistry));
            this.passiveItemRegistry = passiveItemRegistry ?? throw new ArgumentNullException(nameof(passiveItemRegistry));
            this.weaponFactory = weaponFactory ?? throw new ArgumentNullException(nameof(weaponFactory));
            this.passiveItemFactory = passiveItemFactory ?? throw new ArgumentNullException(nameof(passiveItemFactory));
            this.randomProvider = randomProvider ?? throw new ArgumentNullException(nameof(randomProvider));
            
            // 檢查registry的definitions
            if (this.weaponRegistry.All.Count == 0)
            {
                throw new NullReferenceException("weaponRegistry is empty");
            }

            if (this.passiveItemRegistry.All.Count == 0)
            {
                throw new NullReferenceException("passiveItemRegistry is empty");
            }

        }

        public IReadOnlyList<ILevelUpCommand> RollOptions(Character character, int optionCount)
        {
            var build = character.Build;
            if (build == null) throw new ArgumentNullException(nameof(build));
            if (optionCount <= 0) throw new ArgumentOutOfRangeException(nameof(optionCount));

            var pool = BuildCandidatePool(build);
            var rng = randomProvider.Get(RandomStream.General);

            // 不重複抽 optionCount 個（不足就回傳現有數量）。
            var results = new List<ILevelUpCommand>(Math.Min(optionCount, pool.Count));
            for (int pick = 0; pick < optionCount && pool.Count > 0; pick++)
            {
                var chosen = pool.RollDrops(rng);
                if (chosen == null)
                {
                    break;
                }

                results.Add(chosen);
                pool.Remove(chosen);
            }
            
            // 如果不足 optionCount, 用AddGoldCoinCommand填充
            while (results.Count < optionCount)
            {
                results.Add(new AddGoldCoinCommand(character, character.Level * 100));
            }

            return results;
        }

        private List<ILevelUpCommand> BuildCandidatePool(Build build)
        {
            int weaponCount = build.Weapons.Count;
            int passiveCount = build.PassiveItems.Count;
            bool weaponSlotsFull = weaponCount >= MaxWeapons;
            bool passiveSlotsFull = passiveCount >= MaxPassiveItems;

            var candidates = new List<ILevelUpCommand>(weaponRegistry.All.Count + passiveItemRegistry.All.Count);

            for (int i = 0; i < weaponRegistry.All.Count; i++)
            {
                var viewDef = weaponRegistry.All.ElementAt(i);
                var def = viewDef.definition;
                if (def == null) continue;

                var owned = build.Weapons.FirstOrDefault(w => w != null && w.Definition == def);
                bool isOwned = owned != null;
                if (isOwned && owned.Level >= def.maxLevel) continue; // 滿等移出池
                if (!isOwned && weaponSlotsFull) continue; // 新物品且欄位滿

                var cmd = new AddOrUpgradeWeaponCommand(build, viewDef, weaponFactory);

                candidates.Add(cmd);
            }

            for (int i = 0; i < passiveItemRegistry.All.Count; i++)
            {
                PassiveItemDefinition def = passiveItemRegistry.All.ElementAt(i);
                if (def == null) continue;

                var owned = build.PassiveItems.FirstOrDefault(p => p != null && p.Definition == def);
                bool isOwned = owned != null;
                if (isOwned && owned.Level >= def.maxLevel) continue; // 滿等移出池
                if (!isOwned && passiveSlotsFull) continue; // 新物品且欄位滿

                var cmd = new AddOrUpgradePassiveItemCommand(build, def, passiveItemFactory);

                candidates.Add(cmd);
            }

            return candidates;
        }
        
    }
}

