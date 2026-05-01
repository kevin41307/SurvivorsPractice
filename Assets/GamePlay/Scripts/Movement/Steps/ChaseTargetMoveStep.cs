using GamePlay.Scripts.Movement.Ports;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Steps
{
    public sealed class ChaseTargetMoveStep : IMoveStep
    {
        private readonly float moveSpeed;

        public ChaseTargetMoveStep(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
        }

        public Vector2 GetDisplacement(in MoveContext context, GridAgent self)
        {
            var toTarget = context.TargetPosition - context.CurrentPosition;
            var desiredDir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.zero;
            var desired = desiredDir * moveSpeed;
            return desired * context.DeltaTime;
        }
    }
}
