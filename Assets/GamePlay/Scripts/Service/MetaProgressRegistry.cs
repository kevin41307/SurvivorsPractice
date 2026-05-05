using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.MetaProgress.Config;

namespace GamePlay.Scripts.Service
{
    public sealed class MetaProgressRegistry
    {
        private readonly Dictionary<string, MetaPowerUpDefinition> powerUpsById;

        public MetaProgressRegistry(IEnumerable<MetaPowerUpDefinition> powerUpDefinitions = null)
        {
            powerUpsById = new Dictionary<string, MetaPowerUpDefinition>(StringComparer.Ordinal);

            if (powerUpDefinitions == null)
                throw new ArgumentNullException(nameof(powerUpDefinitions));

            var defs = powerUpDefinitions as IReadOnlyCollection<MetaPowerUpDefinition>
                ?? powerUpDefinitions.ToArray();

            if (defs.Count == 0)
                throw new ArgumentException("powerUpDefinitions is empty", nameof(powerUpDefinitions));

            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.id))
                {
                    continue;
                }

                if (powerUpsById.ContainsKey(def.id))
                {
                    throw new InvalidOperationException($"[MetaProgressRegistry] 重複的 MetaPowerUpDefinition id: '{def.id}'");
                }

                powerUpsById.Add(def.id, def);
            }
        }

        public bool TryGetPowerUp(string id, out MetaPowerUpDefinition def)
        {
            return powerUpsById.TryGetValue(id, out def);
        }
        
        public bool Exists(string id)
        {
            return powerUpsById.ContainsKey(id);
        }
        
    }
}

