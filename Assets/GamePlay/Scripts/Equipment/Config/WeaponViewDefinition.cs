using GamePlay.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Equipment.Config
{
    [CreateAssetMenu(fileName = "WeaponViewDefinition", menuName = "Scriptable Objects/WeaponViewDefinition")]
    public class WeaponViewDefinition : GuidScriptableObject
    {
        [Required] public GameObject prefab;

        [Required] public WeaponDefinition definition;
    }
}
