using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.Item.Config;

namespace GamePlay.Scripts.Service
{
    public sealed class PassiveItemRegistry
    {
        private readonly Dictionary<string, PassiveItemDefinition> passiveItemsById;
        public Dictionary<string, PassiveItemDefinition>.ValueCollection All => passiveItemsById.Values;

        public PassiveItemRegistry(IEnumerable<PassiveItemDefinition> passiveItemDefinitions)
        {
            passiveItemsById = new Dictionary<string, PassiveItemDefinition>(StringComparer.Ordinal);

            if (passiveItemDefinitions == null)
                throw new ArgumentNullException(nameof(passiveItemDefinitions));

            var defs = passiveItemDefinitions as IReadOnlyCollection<PassiveItemDefinition>
                ?? passiveItemDefinitions.ToArray();

            if (defs.Count == 0)
                throw new ArgumentException("passiveItemDefinitions is empty", nameof(passiveItemDefinitions));

            foreach (var def in defs)
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