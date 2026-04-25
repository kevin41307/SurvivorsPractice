using UnityEngine;
using GamePlay.Scripts.Actor.Config;

namespace GamePlay.Scripts.Service.Config
{
    [CreateAssetMenu(fileName = "CharacterRegistryDefinition",
        menuName = "Scriptable Objects/CharacterRegistryDefinition")]
    public class CharacterRegistryDefinition : ScriptableObject
    {
        public CharacterDefinition[] characters;
    }
}