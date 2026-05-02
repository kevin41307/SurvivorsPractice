using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    public static class TargetSelectGizmoUtility
    {
        public const int CircleSegments = 32;

        public static void DrawCircleXY(Vector3 center, float radius)
        {
            var step = Mathf.PI * 2f / CircleSegments;
            var prevPt = center + new Vector3(radius, 0f, 0f);
            for (var i = 1; i <= CircleSegments; i++)
            {
                var a = i * step;
                var nextPt = center + new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0f);
                Gizmos.DrawLine(prevPt, nextPt);
                prevPt = nextPt;
            }
        }

        public static void DrawConeXY(Vector3 origin, Vector2 forwardUnit, float length, float halfAngleDeg)
        {
            var halfRad = halfAngleDeg * Mathf.Deg2Rad;
            var cos = Mathf.Cos(halfRad);
            var sin = Mathf.Sin(halfRad);
            var fx = forwardUnit.x;
            var fy = forwardUnit.y;
            var left = new Vector2(fx * cos - fy * sin, fx * sin + fy * cos);
            var right = new Vector2(fx * cos + fy * sin, -fx * sin + fy * cos);

            var o2 = new Vector2(origin.x, origin.y);
            var tipL = o2 + left * length;
            var tipR = o2 + right * length;
            Gizmos.DrawLine(origin, new Vector3(tipL.x, tipL.y, origin.z));
            Gizmos.DrawLine(origin, new Vector3(tipR.x, tipR.y, origin.z));

            var arcStep = Mathf.Max(4, Mathf.CeilToInt(halfAngleDeg / 5f) * 2);
            var totalArc = halfAngleDeg * 2f * Mathf.Deg2Rad;
            var step = totalArc / arcStep;
            var prevArc = new Vector3(tipR.x, tipR.y, origin.z);
            for (var i = 1; i <= arcStep; i++)
            {
                var angle = -halfRad + i * step;
                var c = Mathf.Cos(angle);
                var s = Mathf.Sin(angle);
                var dir = new Vector2(fx * c - fy * s, fx * s + fy * c);
                var p = o2 + dir * length;
                var p3 = new Vector3(p.x, p.y, origin.z);
                Gizmos.DrawLine(prevArc, p3);
                prevArc = p3;
            }
        }

        public static void DrawRectangleXY(
            Vector3 origin,
            Vector2 forwardUnit,
            float forwardLen,
            float lateralW,
            float forwardOffset)
        {
            var lateralUnit = new Vector2(-forwardUnit.y, forwardUnit.x);
            var halfW = lateralW * 0.5f;
            var o = new Vector2(origin.x, origin.y);

            Vector2 Corner(float f, float lat) => o + forwardUnit * f + lateralUnit * lat;

            var f0 = forwardOffset;
            var f1 = forwardOffset + forwardLen;
            var c00 = Corner(f0, -halfW);
            var c01 = Corner(f0, halfW);
            var c10 = Corner(f1, -halfW);
            var c11 = Corner(f1, halfW);
            var z = origin.z;

            void Line(Vector2 a, Vector2 b) =>
                Gizmos.DrawLine(new Vector3(a.x, a.y, z), new Vector3(b.x, b.y, z));

            Line(c00, c01);
            Line(c01, c11);
            Line(c11, c10);
            Line(c10, c00);
        }
    }
}