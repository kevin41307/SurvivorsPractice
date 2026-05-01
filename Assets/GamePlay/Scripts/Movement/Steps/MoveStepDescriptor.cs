using System;
using GamePlay.Scripts.Movement.Ports;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Steps
{
    /// <summary>
    /// 可序列化的步驟描述；runtime 透過 <see cref="Build"/> 產生 <see cref="IMoveStep"/>。
    /// </summary>
    [Serializable]
    public abstract class MoveStepDescriptor
    {
        public abstract IMoveStep Build(in MoveStepResolveContext context);
    }

    [Serializable]
    public sealed class ChaseTargetMoveStepDescriptor : MoveStepDescriptor
    {
        [Min(0f)] public float moveSpeed = 1f;

        public override IMoveStep Build(in MoveStepResolveContext context) =>
            new ChaseTargetMoveStep(moveSpeed);
    }

    [Serializable]
    public sealed class SeparationMoveStepDescriptor : MoveStepDescriptor
    {
        [Min(0f)] public float separationRadius = 0.9f;
        [Min(0f)] public float separationStrength = 1.25f;
        [Min(1)] public int updateIntervalFrames = 20;

        public override IMoveStep Build(in MoveStepResolveContext context) =>
            new SeparationMoveStep(context.SpatialHashWorld, separationRadius, separationStrength, updateIntervalFrames);
    }
}
