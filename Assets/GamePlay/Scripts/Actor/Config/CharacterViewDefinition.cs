using GamePlay.Scripts.Actor.Config;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterViewDefinition", menuName = "Scriptable Objects/CharacterViewDefinition")]
public class CharacterViewDefinition : ScriptableObject
{
    //flyweight?
    public EnemyDefinition definition;
    public GameObject prefab;

}