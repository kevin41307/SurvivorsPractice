using System;
using System.Collections.Generic;
using GamePlay.Scripts.Movement.Ports;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Steps
{
    public sealed class MoveStepPipeline : IMoveStep
    {
        private readonly IReadOnlyList<IMoveStep> steps;

        public MoveStepPipeline(IReadOnlyList<IMoveStep> steps)
        {
            this.steps = steps ?? throw new ArgumentNullException(nameof(steps));
            if (steps.Count == 0)
            {
                throw new ArgumentException("Pipeline requires at least one step.", nameof(steps));
            }
        }

        public Vector2 GetDisplacement(in MoveContext context, GridAgent self)
        {
            var total = Vector2.zero;
            for (var i = 0; i < steps.Count; i++)
            {
                total += steps[i].GetDisplacement(in context, self);
            }

            return total;
        }
    }
}
