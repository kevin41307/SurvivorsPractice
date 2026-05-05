using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Scripts.Equipment.Config;

namespace GamePlay.Scripts.Service
{
    public sealed class WeaponRegistry
    {
        private readonly Dictionary<string, WeaponViewDefinition> weaponsById;
        public Dictionary<string, WeaponViewDefinition>.ValueCollection All => weaponsById.Values;

        public WeaponRegistry(IEnumerable<WeaponViewDefinition> weaponViewDefinitions)
        {
            weaponsById = new Dictionary<string, WeaponViewDefinition>(StringComparer.Ordinal);

            if (weaponViewDefinitions == null)
                throw new ArgumentNullException(nameof(weaponViewDefinitions));

            var defs = weaponViewDefinitions as IReadOnlyCollection<WeaponViewDefinition>
                ?? weaponViewDefinitions.ToArray();

            if (defs.Count == 0)
                throw new ArgumentException("weaponViewDefinitions is empty", nameof(weaponViewDefinitions));

            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.Guid))
                {
                    continue;
                }

                if (weaponsById.ContainsKey(def.Guid))
                {
                    throw new InvalidOperationException($"[WeaponRegistry] 重複的 WeaponViewDefinition guid: '{def.Guid}'");
                }

                weaponsById.Add(def.Guid, def);
            }
        }

        public bool TryGetWeapon(string id, out WeaponViewDefinition def)
        {
            return weaponsById.TryGetValue(id, out def);
        }

        public bool Exists(string id)
        {
            return weaponsById.ContainsKey(id);
        }
    }
}