using UnityEngine;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Equipment.Config;

namespace GamePlay.Scripts.Service.Config
{
    [CreateAssetMenu(
        fileName = "WeaponRegistryDefinition",
        menuName = "Scriptable Objects/WeaponRegistryDefinition")]
    public class WeaponRegistryDefinition : ScriptableObject
    {
        public WeaponDefinition[] definitions;
    }
}

