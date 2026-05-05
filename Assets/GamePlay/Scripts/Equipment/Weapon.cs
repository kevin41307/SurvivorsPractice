using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.Status.Ports;
using Kryz.RPG.Stats.Default;

namespace GamePlay.Scripts.Equipment
{
    /// <summary>
    /// 武器 Domain：傷害／冷卻以 Kryz <see cref="Stat"/> 為權威；<see cref="CooldownRemain"/> 倒數至零後由表現層開火並重置為 <see cref="Stat.FinalValue"/>。
    /// <see cref="UpdateModifiers"/> 自 <see cref="Build"/> 合併 <see cref="StatType.Might"/>／<see cref="StatType.Cooldown"/> 的修飾器；<see cref="KnockbackMultiplier"/> 為表底 float，不吃修飾器；傷害結算管線在 <see cref="WeaponView"/>。
    /// </summary>
    public class Weapon
    {
        public int Level { get; private set; } = 1;
        
        public float CooldownRemain { get; private set; }
        public Stat DamageStat => damageStat;
        public Stat CooldownStat => cooldownStat;
        public float KnockbackMultiplier { get; private set; }

        private Stat damageStat = new();
        private Stat cooldownStat = new();
        private WeaponDefinition definition;

        public WeaponDefinition Definition => definition;
        public bool IsMaxLevel => definition != null && Level >= definition.maxLevel;

        public void Configure(WeaponDefinition definition)
        {
            if (definition == null)
            {
                return;
            }

            this.definition = definition;
            Level = 1;
            ApplyStatsFromDefinition();
            ResetCooldown();
        }

        public bool LevelUp(Build build = null)
        {
            if (definition == null)
            {
                return false;
            }

            if (IsMaxLevel)
            {
                return false;
            }

            Level++;
            ApplyStatsFromDefinition();
            ResetCooldown();
            return true;
        }

        public void ResetCooldown()
        {
            CooldownRemain = cooldownStat.FinalValue;
        }

        void ApplyStatsFromDefinition()
        {
            if (definition == null)
            {
                return;
            }

            damageStat = new Stat(definition.GetDamageStat(Level));
            cooldownStat = new Stat(definition.GetCooldownStat(Level));
            KnockbackMultiplier = definition.GetKnockbackStat();
        }

        public void Tick(float deltaTime)
        {
            if (CooldownRemain > 0f)
            {
                CooldownRemain -= deltaTime;
            }
        }

        public bool TryConsumeCooldown()
        {
            if (CooldownRemain > 0f)
            {
                return false;
            }
            
            CooldownRemain = cooldownStat.FinalValue;
            return true;
        }

        public void UpdateModifiers(Build build)
        {
            if (build == null)
            {
                return;
            }

            damageStat.RemoveModifiersFromSource(build.SourceKey);
            cooldownStat.RemoveModifiersFromSource(build.SourceKey);

            ApplyBuildModifiersToStat(build, StatType.Might, damageStat);
            ApplyBuildModifiersToStat(build, StatType.Cooldown, cooldownStat);
        }

        static void ApplyBuildModifiersToStat(Build build, StatType statType, Stat stat)
        {
            if (!build.StatModifiers.TryGetValue(statType, out var modifiers))
            {
                return;
            }

            foreach (var mod in modifiers)
            {
                stat.AddModifier(mod);
            }
        }


    }
}
