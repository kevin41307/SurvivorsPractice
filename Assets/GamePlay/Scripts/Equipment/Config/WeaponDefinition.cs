using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Scripts.Equipment.Config
{
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "Scriptable Objects/WeaponDefinition")]
    public class WeaponDefinition : ScriptableObject
    {
        [MinValue(0f)] public float baseDamage = 1f;

        [MinValue(0.01f)] public float baseCooldown = 1f;
    }
}
