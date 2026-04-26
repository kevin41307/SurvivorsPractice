using UnityEngine;
using GamePlay.Scripts.Actor.Config;

namespace GamePlay.Scripts.Service.Config
{
    [CreateAssetMenu(fileName = "CharacterRegistryDefinition",
        menuName = "Scriptable Objects/Service/CharacterRegistryDefinition")]
    public class CharacterRegistryDefinition : ScriptableObject
    {
        public CharacterDefinition[] characters;
    }
}