using System.Collections.Generic;
using GamePlay.Scripts.Core;
using GamePlay.Scripts.Movement.Ports;
using GamePlay.Scripts.Movement.Steps;
using GamePlay.Scripts.Ports;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Actor.Config
{
    [CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Scriptable Objects/EnemyDefinition")]
    public class EnemyDefinition : SerializedScriptableObject
    {
        
        
        [Required, SerializeReference]
        IDropPolicy dropPolicy;

        [Required, SerializeReference]
        List<MoveStepDescriptor> movePipeline = new();

        public IMoveStep BuildMoveStepPipeline(SpatialHashWorld world)
        {
            var ctx = new MoveStepResolveContext(world);
            var built = new List<IMoveStep>(movePipeline.Count);
            foreach (var d in movePipeline)
            {
                if (d == null)
                {
                    continue;
                }

                built.Add(d.Build(in ctx));
            }

            if (built.Count == 0)
            {
                return null;
            }

            return built.Count == 1 ? built[0] : new MoveStepPipeline(built);
        }

        public float GetHealthPointStat(int level)
        {
            return 1;
        }
    }
}
