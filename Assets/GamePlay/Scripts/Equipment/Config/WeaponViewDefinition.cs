using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Equipment.Config
{
    [CreateAssetMenu(fileName = "WeaponViewDefinition", menuName = "Scriptable Objects/WeaponViewDefinition")]
    public class WeaponViewDefinition : ScriptableObject
    {
        [Required] public GameObject prefab;
    }
}