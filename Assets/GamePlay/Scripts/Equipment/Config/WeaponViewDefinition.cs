using UnityEngine;

namespace GamePlay.Scripts.Equipment.Config
{
    [CreateAssetMenu(fileName = "WeaponViewDefinition", menuName = "Scriptable Objects/WeaponViewDefinition")]
    public class WeaponViewDefinition : ScriptableObject
    {
        public GameObject prefab;
    }
}