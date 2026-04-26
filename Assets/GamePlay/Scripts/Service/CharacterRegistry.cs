using System;
using System.Collections.Generic;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Service.Config;
using UnityEngine;

namespace GamePlay.Scripts.Service
{
    public class CharacterRegistry
    {
        private readonly CharacterRegistryDefinition characterRegistryDefinition;
        private readonly Dictionary<string, CharacterDefinition> characterByGuid;
          
        public CharacterRegistry(CharacterRegistryDefinition characterRegistryDefinition)
        {
            this.characterRegistryDefinition = characterRegistryDefinition;
            characterByGuid = BuildIndex(characterRegistryDefinition);
        }

        public bool TryGet(Guid id, out CharacterDefinition definition)
        {
            return characterByGuid.TryGetValue(id.ToString("N"), out definition);
        }

        public bool TryGet(string guid, out CharacterDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                definition = null;
                return false;
            }

            var normalized = NormalizeGuidString(guid);
            return characterByGuid.TryGetValue(normalized, out definition);
        }

        private static Dictionary<string, CharacterDefinition> BuildIndex(CharacterRegistryDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            var characters = definition.characters;
            if (characters == null)
            {
                return new Dictionary<string, CharacterDefinition>(0);
            }

            var map = new Dictionary<string, CharacterDefinition>(characters.Length);
            for (var i = 0; i < characters.Length; i++)
            {
                var character = characters[i];
                if (character == null)
                {
                    Debug.LogError($"CharacterRegistryDefinition has null CharacterDefinition at index {i}.");
                    continue;
                }

                var rawGuid = character.Guid;
                if (string.IsNullOrWhiteSpace(rawGuid))
                {
                    Debug.LogError($"CharacterDefinition '{character.name}' has empty guid.");
                    continue;
                }

                if (!Guid.TryParse(rawGuid, out var parsed))
                {
                    Debug.LogError($"CharacterDefinition '{character.name}' has invalid guid '{rawGuid}'.");
                    continue;
                }

                var key = parsed.ToString("N");
                if (map.ContainsKey(key))
                {
                    Debug.LogError($"Duplicate character guid '{rawGuid}' (normalized '{key}') found on '{character.name}'.");
                    continue;
                }

                map.Add(key, character);
            }

            return map;
        }

        private static string NormalizeGuidString(string guid)
        {
            if (Guid.TryParse(guid, out var parsed))
            {
                return parsed.ToString("N");
            }

            return guid.Trim();
        }
    }
}