using System;
using System.Collections.Generic;
using GamePlay.Scripts.Item.Config;
using UnityEngine;

namespace GamePlay.Scripts.Service
{
    public sealed class PassiveItemRegistry
    {
        private readonly Dictionary<string, PassiveItemDefinition> passiveItemsById;
        public Dictionary<string, PassiveItemDefinition>.ValueCollection All => passiveItemsById.Values;

        public PassiveItemRegistry(IEnumerable<PassiveItemDefinition> passiveItemDefinitions)
        {
            if (passiveItemDefinitions == null)
                throw new NullReferenceException("passiveItemDefinitions is null");
            
            passiveItemsById = new Dictionary<string, PassiveItemDefinition>(StringComparer.Ordinal);

            foreach (var def in passiveItemDefinitions)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.Guid))
                {
                    continue;
                }

                if (passiveItemsById.ContainsKey(def.Guid))
                {
                    throw new InvalidOperationException($"[PassiveItemRegistry] 重複的 PassiveItemDefinition guid: '{def.Guid}'");
                }

                passiveItemsById.Add(def.Guid, def);
            }
        }

        public bool TryGetPassiveItem(string id, out PassiveItemDefinition def)
        {
            return passiveItemsById.TryGetValue(id, out def);
        }

        public bool Exists(string id)
        {
            return passiveItemsById.ContainsKey(id);
        }
    }
}