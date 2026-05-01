using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Movement.Ports;
using SpatialHash2D;
using GamePlay.Scripts.Service.Ports;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    public sealed class EnemyMovementController : MonoBehaviour
    {
        [Inject] private readonly SpatialHashWorld spatialHashWorld;
        [Inject] private readonly IPlayerLocator playerLocator;

        private EnemyView enemyView;
        private GridAgent agent;

        private IMoveStep cachedMovePipeline;
        private EnemyDefinition cachedPipelineDefinition;

        private void Awake()
        {
            enemyView = GetComponent<EnemyView>();
        }

        private void OnEnable()
        {
            agent = spatialHashWorld?.Registry.Register(transform);
            cachedMovePipeline = null;
            cachedPipelineDefinition = null;
        }

        private void OnDisable()
        {
            agent = null;
            cachedMovePipeline = null;
            cachedPipelineDefinition = null;
        }

        private IMoveStep ResolveMovePipeline(EnemyDefinition definition)
        {
            if (definition == null)
            {
                return null;
            }

            if (cachedPipelineDefinition != definition || cachedMovePipeline == null)
            {
                cachedPipelineDefinition = definition;
                cachedMovePipeline = definition.BuildMoveStepPipeline(spatialHashWorld);
            }

            return cachedMovePipeline;
        }

        private void Update()
        {
            if (spatialHashWorld == null || playerLocator?.Player?.CharacterView == null)
            {
                return;
            }

            if (enemyView == null || enemyView.ViewDefinition == null || enemyView.ViewDefinition.definition == null)
            {
                return;
            }

            var pipeline = ResolveMovePipeline(enemyView.ViewDefinition.definition);
            if (pipeline == null)
            {
                return;
            }


            var pos3 = transform.position;
            var current = new Vector2(pos3.x, pos3.y);

            var playerPos3 = playerLocator.Player.CharacterView.transform.position;
            var target = new Vector2(playerPos3.x, playerPos3.y);

            var context = new MoveContext(
                entityId: agent?.Id ?? 0,
                frameCount: spatialHashWorld.FrameCount,
                deltaTime: Time.deltaTime,
                currentPosition: current,
                targetPosition: target);

            if (agent == null)
                return;
            
            var displacement = pipeline.GetDisplacement(in context, agent);

            transform.position = new Vector3(
                pos3.x + displacement.x,
                pos3.y + displacement.y,
                pos3.z);
        }
    }
}
