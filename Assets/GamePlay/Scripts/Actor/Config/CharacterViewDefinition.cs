using GamePlay.Scripts.Actor.Config;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterViewDefinition", menuName = "Scriptable Objects/CharacterViewDefinition")]
public class CharacterViewDefinition : SerializedScriptableObject
{
    [Required] public GameObject prefab;
    //flyweight?
    [Required] public EnemyDefinition definition;

}