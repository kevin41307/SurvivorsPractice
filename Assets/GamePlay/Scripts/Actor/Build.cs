using System.Collections.Generic;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Status.Buff;
using GamePlay.Scripts.Status.Ports;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;
using System.Linq;
using UnityEngine;

namespace GamePlay.Scripts.Actor
{
    public class Build
    {
        readonly object buildModifierSourceKey = new();                
        public object SourceKey => buildModifierSourceKey;
        public List<PassiveItem> PassiveItems { get; private set; } = new();
        public List<PowerUp> PowerUps { get; private set; } = new();
        public List<Weapon> Weapons { get; private set; } = new();

        private readonly Dictionary<StatType, List<StatModifier<StatModifierData>>> powerUpModifierCache = new();
        private readonly Dictionary<StatType, List<StatModifier<StatModifierData>>> passiveItemModifierCache = new();
        
        private readonly Dictionary<StatType, List<StatModifier<StatModifierData>>> statModifierCache = new();
        public IReadOnlyDictionary<StatType, List<StatModifier<StatModifierData>>> StatModifiers => statModifierCache;
        
        

        public void AddPassiveItem(PassiveItem item)
        {
            if (item == null)
            {
                Debug.LogError("PassiveItem is null");
                return;
            }

            var existing = PassiveItems.FirstOrDefault(x => x.Definition == item.Definition);
            if (existing != null)
            {
                existing.Upgrade();
                RefreshPassiveItemCache();
                return;
            }

            PassiveItems.Add(item);
            RefreshPassiveItemCache();
        }
        

        public void AddPowerUp(PowerUp item)
        {
             
            if (item == null)
            {
                Debug.LogError("PowerUp is null");
                return;
            }
            
            // 如果列表中已經有相同的PowerUp, 則return
            if (PowerUps.Exists(x => x.Definition == item.Definition))
            {
                Debug.LogError("PowerUp already exists");
                return;
            }            
            
            PowerUps.Add(item);
            RefreshPowerUpCache();
        }
        


        public void AddWeapon(Weapon item)
        {
            if (item == null)
            {
                Debug.LogError("Weapon is null");
                return;
            }

            Weapons.Add(item);
            item.UpdateModifiers(this);
        }

        public void AddBuild(Build other)
        {
            other.PassiveItems.ForEach(AddPassiveItem);
            other.PowerUps.ForEach(AddPowerUp);
            other.Weapons.ForEach(AddWeapon);
        }

        private void RefreshPassiveItemCache()
        {
            passiveItemModifierCache.Clear();
            foreach (var passive in PassiveItems)
            {
                if (passive?.Definition == null)
                {
                    continue;
                }

                float bonus = passive.GetTotalBonus();
                var statType = passive.Definition.statType;
                var statModifierType = passive.Definition.statModifierType;
                if (!passiveItemModifierCache.TryGetValue(statType, out var statModifiers))
                {
                    statModifiers = new();
                    passiveItemModifierCache[statType] = statModifiers;
                }

                statModifiers.Add(new StatModifier<StatModifierData>(
                    bonus,
                    new StatModifierData(statModifierType, buildModifierSourceKey)));
            }
            RefreshStatModifierCache();
        }        

        private void RefreshPowerUpCache()
        {
            powerUpModifierCache.Clear();
            foreach (var powerUp in PowerUps)
            {
                if (powerUp?.Definition == null)
                {
                    continue;
                }

                float bonus = powerUp.GetTotalBonus();
                var statType = powerUp.Definition.statType;
                var statModifierType = powerUp.Definition.statModifierType;
                if (!powerUpModifierCache.TryGetValue(statType, out var statModifiers))
                {
                    statModifiers = new();
                    powerUpModifierCache[statType] = statModifiers;
                }

                statModifiers.Add(new StatModifier<StatModifierData>(
                    bonus,
                    new StatModifierData(statModifierType, buildModifierSourceKey)));
            }

            RefreshStatModifierCache();
        }

        private void RefreshStatModifierCache()
        {
            statModifierCache.Clear();
            AppendIntoMergedStatCache(passiveItemModifierCache);
            AppendIntoMergedStatCache(powerUpModifierCache);
            NotifyObservers();
        }

        private void AppendIntoMergedStatCache(
            IReadOnlyDictionary<StatType, List<StatModifier<StatModifierData>>> source)
        {
            foreach (var kvp in source)
            {
                if (!statModifierCache.TryGetValue(kvp.Key, out var merged))
                {
                    merged = new();
                    statModifierCache[kvp.Key] = merged;
                }

                merged.AddRange(kvp.Value);
            }
        }

        private void NotifyObservers()
        {
            foreach (var weapon in Weapons)
            {
                weapon.UpdateModifiers(this);
            }
            
        }
 
    }
}