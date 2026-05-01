using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    public enum TargetSelectMode
    {
        Nearest,
        Cone,
        Rectangle,
    }

    /// <summary>
    /// 掛在施放者 Prefab：提示圖、Runtime hint、Scene Gizmo、選取查詢。幾何為 XY；朝前見 <see cref="GetPlanarForward"/>。
    /// </summary>
    public sealed class TargetSelectView : MonoBehaviour
    {
        const float EpsilonSqr = 1e-6f;
        const float BoundsEpsilon = 1e-6f;
        const float ShaderFullDiskHalfAngleDegrees = 180f;

        [Title("企劃／編輯參考")]
        [PreviewField(Height = 64)]
        [SerializeField]
        Sprite hintSprite;

        [SerializeField]
        TargetSelectMode mode = TargetSelectMode.Nearest;

        [ShowIf("@mode == TargetSelectMode.Nearest")]
        [MinValue(0.01f)]
        [SerializeField]
        float maxRadius = 5f;

        [ShowIf("@mode == TargetSelectMode.Cone")]
        [MinValue(0.01f)]
        [SerializeField]
        float coneLength = 5f;

        /// <summary>與前向夾角之半角（度），總視野為 2×此值。</summary>
        [ShowIf("@mode == TargetSelectMode.Cone")]
        [Range(0f, 180f)]
        [SerializeField]
        float halfAngleDegrees = 45f;

        [ShowIf("@mode == TargetSelectMode.Rectangle")]
        [MinValue(0.01f)]
        [SerializeField]
        float forwardLength = 4f;

        [ShowIf("@mode == TargetSelectMode.Rectangle")]
        [MinValue(0.01f)]
        [SerializeField]
        float lateralWidth = 3f;

        /// <summary>沿前向平移矩形區間起點（與原點同側邊中點為基準）。</summary>
        [ShowIf("@mode == TargetSelectMode.Rectangle")]
        [SerializeField]
        float centerOffsetAlongForward;

        [Title("Gizmo")]
        [SerializeField]
        Color gizmoColor = new Color(0.2f, 0.85f, 1f, 0.9f);

        [Title("Hint (Runtime)")]
        [SerializeField]
        bool autoCreateHintRenderer = true;
        
        private SpriteRenderer hintRenderer;

        [Required, SerializeField]
        Material circleConeHintMaterial;
        
        [Required, SerializeField]
        Material rectangleHintMaterial;

        [SerializeField]
        Color hintColor = new Color(0.2f, 0.85f, 1f, 0.45f);

        [SerializeField]
        float hintZOffset;

        [Tooltip("應大於角色／敵人 SpriteRenderer 的 Sorting Order（專案內多為 0），hint 才會畫在最上層。")]
        [SerializeField]
        int hintSortingOrder = 50;

        bool isHintVisible;
        MaterialPropertyBlock hintPropertyBlock;

        const int CircleSegments = 32;
        static readonly int HalfAngleId = Shader.PropertyToID("_HalfAngle");

        public Sprite HintSprite => hintSprite;
        public TargetSelectMode Mode => mode;
        public float MaxRadius => maxRadius;
        public float ConeLength => coneLength;
        public float HalfAngleDegrees => halfAngleDegrees;
        public float ForwardLength => forwardLength;
        public float LateralWidth => lateralWidth;
        public float CenterOffsetAlongForward => centerOffsetAlongForward;
        public bool IsHintVisible => isHintVisible;

        void Awake()
        {
            EnsureHintRenderer();
        }

        public void ShowHint()
        {
            EnsureHintRenderer();
            isHintVisible = true;
            RefreshHint();
        }

        public void HideHint()
        {
            isHintVisible = false;
            if (hintRenderer == null)
            {
                return;
            }

            hintRenderer.enabled = false;
            hintRenderer.SetPropertyBlock(null);
        }

        public void UpdateHint()
        {
            if (!isHintVisible)
            {
                return;
            }

            RefreshHint();
        }

        public bool TrySelectNearest(SpatialHashWorld world, out ITargetable target) =>
            TrySelectNearest(world, ToPlane(transform.position), GetPlanarForward(), out target);

        public bool TrySelectNearest(SpatialHashWorld world, Vector2 origin, Vector2 forward, out ITargetable target)
        {
            _ = forward;
            target = null;
            if (world == null || mode != TargetSelectMode.Nearest)
            {
                return false;
            }

            var radiusSqr = maxRadius * maxRadius;
            var bestSqr = float.MaxValue;
            ITargetable best = null;

            foreach (var agent in world.Registry.Agents)
            {
                if (!TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var sqr = (ToPlane(t.Transform.position) - origin).sqrMagnitude;
                if (sqr > radiusSqr || sqr >= bestSqr)
                {
                    continue;
                }

                bestSqr = sqr;
                best = t;
            }

            target = best;
            return best != null;
        }

        public void SelectAllInShape(SpatialHashWorld world, List<ITargetable> results) =>
            SelectAllInShape(world, ToPlane(transform.position), GetPlanarForward(), results);

        public void SelectAllInShape(SpatialHashWorld world, Vector2 origin, Vector2 forward, List<ITargetable> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            results.Clear();
            if (world == null)
            {
                return;
            }

            switch (mode)
            {
                case TargetSelectMode.Nearest:
                    if (TrySelectNearest(world, origin, forward, out var one) && one != null)
                    {
                        results.Add(one);
                    }
                    break;
                case TargetSelectMode.Cone:
                    if (!TryNormalizeForward(forward, out var uCone))
                    {
                        return;
                    }

                    SelectCone(world, origin, uCone, results);
                    break;
                case TargetSelectMode.Rectangle:
                    if (!TryNormalizeForward(forward, out var uRect))
                    {
                        return;
                    }

                    SelectRectangle(world, origin, uRect, results);
                    break;
            }
        }

        public Vector2 GetPlanarForward()
        {
            var forward = new Vector2(transform.right.x, transform.right.y);
            if (forward.sqrMagnitude < EpsilonSqr)
            {
                return Vector2.right;
            }

            forward.Normalize();
            return forward;
        }

        void RefreshHint()
        {
            ApplyHintVisuals();
            UpdateHintTransform();
        }

        void EnsureHintRenderer()
        {
            if (hintRenderer != null || !autoCreateHintRenderer)
            {
                return;
            }

            var go = new GameObject("TargetHint");
            go.transform.SetParent(transform, worldPositionStays: false);
            hintRenderer = go.AddComponent<SpriteRenderer>();
            hintRenderer.enabled = false;
        }

        void ApplyHintVisuals()
        {
            if (hintRenderer == null)
            {
                return;
            }

            hintRenderer.sprite = hintSprite;
            hintRenderer.color = hintColor;
            hintRenderer.sortingOrder = hintSortingOrder;

            var hasSprite = hintSprite != null;
            if (!isHintVisible || !hasSprite)
            {
                hintRenderer.enabled = false;
                return;
            }

            switch (mode)
            {
                case TargetSelectMode.Nearest:
                case TargetSelectMode.Cone:
                    if (circleConeHintMaterial == null)
                    {
                        hintRenderer.enabled = false;
                        return;
                    }

                    hintRenderer.sharedMaterial = circleConeHintMaterial;
                    hintPropertyBlock ??= new MaterialPropertyBlock();
                    hintRenderer.GetPropertyBlock(hintPropertyBlock);
                    hintPropertyBlock.SetFloat(
                        HalfAngleId,
                        mode == TargetSelectMode.Nearest ? ShaderFullDiskHalfAngleDegrees : halfAngleDegrees);
                    hintRenderer.SetPropertyBlock(hintPropertyBlock);
                    hintRenderer.enabled = true;
                    break;

                case TargetSelectMode.Rectangle:
                    hintRenderer.sharedMaterial = rectangleHintMaterial;
                    hintRenderer.SetPropertyBlock(null);
                    hintRenderer.enabled = true;
                    break;
            }
        }

        void UpdateHintTransform()
        {
            if (hintRenderer == null || !hintRenderer.enabled || hintSprite == null)
            {
                return;
            }

            var origin3 = transform.position;
            var hintTr = hintRenderer.transform;
            var bounds = hintSprite.bounds.size;

            switch (mode)
            {
                case TargetSelectMode.Rectangle:
                    PlaceRectangleHint(origin3, hintTr, bounds);
                    break;
                case TargetSelectMode.Nearest:
                case TargetSelectMode.Cone:
                    PlaceCircleOrConeHint(origin3, hintTr, bounds);
                    break;
            }
        }

        void PlaceRectangleHint(Vector3 origin3, Transform hintTr, Vector3 spriteBoundsSize)
        {
            var forward = GetPlanarForward();
            var centerFwd = centerOffsetAlongForward + forwardLength * 0.5f;
            var center2 = new Vector2(origin3.x, origin3.y) + forward * centerFwd;

            hintTr.position = new Vector3(center2.x, center2.y, origin3.z + hintZOffset);
            hintTr.rotation = RotationRightToPlanarForward(forward);

            var sx = spriteBoundsSize.x > BoundsEpsilon ? forwardLength / spriteBoundsSize.x : 1f;
            var sy = spriteBoundsSize.y > BoundsEpsilon ? lateralWidth / spriteBoundsSize.y : 1f;
            hintTr.localScale = new Vector3(sx, sy, 1f);
        }

        void PlaceCircleOrConeHint(Vector3 origin3, Transform hintTr, Vector3 spriteBoundsSize)
        {
            var forward = GetPlanarForward();
            hintTr.position = new Vector3(origin3.x, origin3.y, origin3.z + hintZOffset);
            hintTr.rotation = RotationRightToPlanarForward(forward);

            var radius = mode == TargetSelectMode.Nearest ? maxRadius : coneLength;
            var diameter = radius * 2f;
            var s = spriteBoundsSize.x > BoundsEpsilon ? diameter / spriteBoundsSize.x : 1f;
            hintTr.localScale = new Vector3(s, s, 1f);
        }

        static Quaternion RotationRightToPlanarForward(Vector2 forward) =>
            Quaternion.FromToRotation(Vector3.right, new Vector3(forward.x, forward.y, 0f));

        void OnDrawGizmosSelected()
        {
            var origin = transform.position;
            var forward = GetPlanarForward();

            var prev = Gizmos.color;
            Gizmos.color = gizmoColor;

            switch (mode)
            {
                case TargetSelectMode.Nearest:
                    DrawCircleXY(origin, maxRadius);
                    break;
                case TargetSelectMode.Cone:
                    DrawConeXY(origin, forward, coneLength, halfAngleDegrees);
                    break;
                case TargetSelectMode.Rectangle:
                    DrawRectangleXY(origin, forward, forwardLength, lateralWidth, centerOffsetAlongForward);
                    break;
            }

            Gizmos.color = prev;
        }

        static void DrawCircleXY(Vector3 center, float radius)
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

        static void DrawConeXY(Vector3 origin, Vector2 forwardUnit, float length, float halfAngleDeg)
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

        static void DrawRectangleXY(Vector3 origin, Vector2 forwardUnit, float forwardLen, float lateralW, float forwardOffset)
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

        void SelectCone(SpatialHashWorld world, Vector2 origin, Vector2 forwardUnit, List<ITargetable> results)
        {
            var lenSqr = coneLength * coneLength;

            foreach (var agent in world.Registry.Agents)
            {
                if (!TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var delta = ToPlane(t.Transform.position) - origin;
                var distSqr = delta.sqrMagnitude;
                if (distSqr > lenSqr)
                {
                    continue;
                }

                if (distSqr <= EpsilonSqr)
                {
                    results.Add(t);
                    continue;
                }

                if (Vector2.Angle(forwardUnit, delta) > halfAngleDegrees)
                {
                    continue;
                }

                results.Add(t);
            }
        }

        void SelectRectangle(SpatialHashWorld world, Vector2 origin, Vector2 forwardUnit, List<ITargetable> results)
        {
            var lateralUnit = new Vector2(-forwardUnit.y, forwardUnit.x);
            var halfW = lateralWidth * 0.5f;
            var f0 = centerOffsetAlongForward;
            var f1 = centerOffsetAlongForward + forwardLength;

            foreach (var agent in world.Registry.Agents)
            {
                if (!TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var delta = ToPlane(t.Transform.position) - origin;
                var fwd = Vector2.Dot(delta, forwardUnit);
                var lat = Vector2.Dot(delta, lateralUnit);
                if (fwd < f0 || fwd > f1 || Mathf.Abs(lat) > halfW)
                {
                    continue;
                }

                results.Add(t);
            }
        }

        static bool TryGetTargetable(GridAgent agent, out ITargetable target)
        {
            target = null;
            if (agent?.Transform == null)
            {
                return false;
            }

            return agent.Transform.TryGetComponent<ITargetable>(out target);
        }

        static bool TryNormalizeForward(Vector2 forward, out Vector2 normalized)
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

        static Vector2 ToPlane(Vector3 position) => new Vector2(position.x, position.y);
    }
}
