using GamePlay.Scripts.Movement.Ports;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Steps
{
    public sealed class SeparationMoveStep : IMoveStep
    {
        private readonly SpatialHashWorld world;
        private readonly float separationRadius;
        private readonly float separationStrength;
        private readonly int updateIntervalFrames;
        private readonly float radiusSqr;

        public SeparationMoveStep(
            SpatialHashWorld world,
            float separationRadius,
            float separationStrength,
            int updateIntervalFrames)
        {
            this.world = world;
            this.separationRadius = separationRadius;
            this.separationStrength = separationStrength;
            this.updateIntervalFrames = updateIntervalFrames;
            radiusSqr = SpatialHashGrid2D.RadiusToSqr(separationRadius);
        }

        public Vector2 GetDisplacement(in MoveContext context, GridAgent self)
        {
            if (world == null || self == null || !ShouldUpdateSeparation(self.Id, context.FrameCount))
            {
                return Vector2.zero;
            }

            if (radiusSqr <= 0f)
            {
                return Vector2.zero;
            }

            var current = context.CurrentPosition;
            var grid = world.Grid;
            var neighbors = grid.QueryNeighbors(current);

            var push = Vector2.zero;
            for (var i = 0; i < neighbors.Count; i++)
            {
                var other = neighbors[i];
                if (other == null || other == self)
                {
                    continue;
                }

                var otherPos = other.Position2D;
                var delta = current - otherPos;
                var distSqr = delta.sqrMagnitude;
                if (distSqr <= 0.000001f || distSqr >= radiusSqr)
                {
                    continue;
                }

                var dist = Mathf.Sqrt(distSqr);
                var t = 1f - Mathf.Clamp01(dist / separationRadius);
                push += (delta / dist) * t;
            }

            return push * (separationStrength * context.DeltaTime);
        }

        private bool ShouldUpdateSeparation(int entityId, int frameCount)
        {
            if (updateIntervalFrames <= 1)
            {
                return true;
            }

            return (entityId % updateIntervalFrames) == (frameCount % updateIntervalFrames);
        }
    }
}
