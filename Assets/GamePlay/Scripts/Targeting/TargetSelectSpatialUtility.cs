using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{

    public static class TargetSelectSpatialUtility
    {
        public const float EpsilonSqr = 1e-6f;
        public const float BoundsEpsilon = 1e-6f;

        public static Quaternion RotationRightToPlanarForward(Vector2 forward) =>
            Quaternion.FromToRotation(Vector3.right, new Vector3(forward.x, forward.y, 0f));

        public static bool TryGetTargetable(GridAgent agent, out ITargetable target)
        {
            target = null;
            if (agent?.Transform == null)
            {
                return false;
            }

            return agent.Transform.TryGetComponent<ITargetable>(out target);
        }

        public static bool TryNormalizeForward(Vector2 forward, out Vector2 normalized)
        {
            var sqr = forward.sqrMagnitude;
            if (sqr < EpsilonSqr)
            {
                normalized = default;
                return false;
            }

            normalized = forward / Mathf.Sqrt(sqr);
            return true;
        }

        public static Vector2 ToPlane(Vector3 position) => new Vector2(position.x, position.y);

        /// <summary>
        /// 以 caster 為原點、<paramref name="forwardUnit"/> 為前向之軸對齊矩形（沿前向 [forwardMin, forwardMax]、側向 ±halfLateral）在 XY 上的外包 AABB。
        /// </summary>
        public static void GetOrientedRectangleWorldAabb(
            Vector2 origin,
            Vector2 forwardUnit,
            float forwardMin,
            float forwardMax,
            float halfLateral,
            out Vector2 worldMin,
            out Vector2 worldMax)
        {
            var lateralUnit = new Vector2(-forwardUnit.y, forwardUnit.x);
            Vector2 Corner(float f, float lat) => origin + forwardUnit * f + lateralUnit * lat;
            var c00 = Corner(forwardMin, -halfLateral);
            var c01 = Corner(forwardMin, halfLateral);
            var c10 = Corner(forwardMax, -halfLateral);
            var c11 = Corner(forwardMax, halfLateral);
            worldMin = Vector2.Min(Vector2.Min(c00, c01), Vector2.Min(c10, c11));
            worldMax = Vector2.Max(Vector2.Max(c00, c01), Vector2.Max(c10, c11));
        }
    }
}