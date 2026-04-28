using System;
using System.Collections.Generic;
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
            {
                return;
            }

            foreach (var def in powerUpDefinitions)
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
        
    }
}

