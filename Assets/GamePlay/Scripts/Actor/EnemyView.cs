using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public class EnemyView : MonoBehaviour
    {
        public Enemy Enemy { get; set; }

        [Inject]
        public void Construct(Enemy enemy)
        {
            Enemy = enemy;
        }


        
    }
}