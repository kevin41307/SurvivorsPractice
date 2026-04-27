using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class CharacterView : MonoBehaviour
    {
        [Inject]
        public Character Character { get; set; }
        
    }
}