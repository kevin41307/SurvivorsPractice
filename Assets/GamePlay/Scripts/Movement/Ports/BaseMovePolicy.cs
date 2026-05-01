using System;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Ports
{
    [Serializable]
    public abstract class BaseMovePolicy
    {
        [Min(0f)] public float moveSpeed = 1f;

        public abstract Vector2 CalculateDisplacement(in MoveContext context);
    }

    /// <summary>
    /// Intent:
    /// 提供可被 SerializeReference 指到的簡單移動策略，避免舊資產（EnemyDefinition.asset）引用遺失型別。
    ///
    /// Invariant:
    /// - 類別全名必須維持為 GamePlay.Scripts.Movement.Ports.DebugMovePolicy
    ///
    /// Why:
    /// 這個類別曾存在且被資產序列化引用；保留能避免 Unity 顯示 Missing types。
    /// </summary>
    [Serializable]
    public sealed class DebugMovePolicy : BaseMovePolicy
    {
        public int x;
        private int y;

        public Vector2 position;

        public override Vector2 CalculateDisplacement(in MoveContext context)
        {
            var toTarget = position - context.CurrentPosition;
            var dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.zero;
            return dir * (moveSpeed * context.DeltaTime);
        }
    }
    
    public readonly struct MoveContext
    {
        public MoveContext(
            int entityId,
            int frameCount,
            float deltaTime,
            Vector2 currentPosition,
            Vector2 targetPosition)
        {
            EntityId = entityId;
            FrameCount = frameCount;
            DeltaTime = deltaTime;
            CurrentPosition = currentPosition;
            TargetPosition = targetPosition;
        }

        public int EntityId { get; }
        public int FrameCount { get; }
        public float DeltaTime { get; }
        public Vector2 CurrentPosition { get; }
        public Vector2 TargetPosition { get; }
    }
}