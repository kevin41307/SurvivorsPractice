using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    [Serializable]
    public sealed class RectangleSelectView : TargetSelectStrategyBase
    {
        [MinValue(0.01f)]
        [SerializeField]
        float forwardLength = 4f;

        [MinValue(0.01f)]
        [SerializeField]
        float lateralWidth = 3f;

        /// <summary>沿前向平移矩形區間起點（與原點同側邊中點為基準）。</summary>
        [SerializeField]
        float centerOffsetAlongForward;

        public RectangleSelectView()
        {
        }

        public RectangleSelectView(float forwardLengthValue, float lateralWidthValue, float centerOffsetAlongForwardValue)
        {
            forwardLength = forwardLengthValue;
            lateralWidth = lateralWidthValue;
            centerOffsetAlongForward = centerOffsetAlongForwardValue;
        }

        public float ForwardLength => forwardLength;
        public float LateralWidth => lateralWidth;
        public float CenterOffsetAlongForward => centerOffsetAlongForward;

        public override void SelectAllInShape(
            SpatialHashWorld world,
            Vector2 origin,
            Vector2 forward,
            List<ITargetable> results)
        {
            if (!TargetSelectSpatialUtility.TryNormalizeForward(forward, out var uRect))
            {
                return;
            }

            SelectRectangle(world, origin, uRect, results);
        }

        void SelectRectangle(SpatialHashWorld world, Vector2 origin, Vector2 forwardUnit, List<ITargetable> results)
        {
            var lateralUnit = new Vector2(-forwardUnit.y, forwardUnit.x);
            var halfW = lateralWidth * 0.5f;
            var f0 = centerOffsetAlongForward;
            var f1 = centerOffsetAlongForward + forwardLength;

            TargetSelectSpatialUtility.GetOrientedRectangleWorldAabb(
                origin,
                forwardUnit,
                f0,
                f1,
                halfW,
                out var aabbMin,
                out var aabbMax);
            var candidates = world.Grid.QueryAgentsInWorldAabb(aabbMin, aabbMax);

            foreach (var agent in candidates)
            {
                if (!TargetSelectSpatialUtility.TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var delta = TargetSelectSpatialUtility.ToPlane(t.Transform.position) - origin;
                var fwd = Vector2.Dot(delta, forwardUnit);
                var lat = Vector2.Dot(delta, lateralUnit);
                if (fwd < f0 || fwd > f1 || Mathf.Abs(lat) > halfW)
                {
                    continue;
                }

                results.Add(t);
            }
        }

        public override void ApplyHintVisuals(TargetSelectHintApplyContext ctx)
        {
            if (ctx.HintRenderer == null)
            {
                return;
            }

            ctx.HintRenderer.sprite = ctx.HintSprite;
            ctx.HintRenderer.color = ctx.HintColor;
            ctx.HintRenderer.sortingOrder = ctx.HintSortingOrder;

            var hasSprite = ctx.HintSprite != null;
            if (!ctx.IsHintVisible || !hasSprite)
            {
                ctx.HintRenderer.enabled = false;
                return;
            }

            if (ctx.RectangleHintMaterial == null)
            {
                ctx.HintRenderer.enabled = false;
                return;
            }

            ctx.HintRenderer.sharedMaterial = ctx.RectangleHintMaterial;
            ctx.HintRenderer.SetPropertyBlock(null);
            ctx.HintRenderer.enabled = true;
        }

        public override void UpdateHintTransform(in TargetSelectHintTransformContext ctx)
        {
            var hintTr = ctx.HintTransform;
            if (hintTr == null)
            {
                return;
            }

            var origin3 = ctx.CasterPosition;
            var forward = ctx.PlanarForwardUnit;
            var centerFwd = centerOffsetAlongForward + forwardLength * 0.5f;
            var center2 = new Vector2(origin3.x, origin3.y) + forward * centerFwd;

            hintTr.position = new Vector3(center2.x, center2.y, origin3.z + ctx.HintZOffset);
            hintTr.rotation = TargetSelectSpatialUtility.RotationRightToPlanarForward(forward);

            var sx = ctx.SpriteBoundsSize.x > TargetSelectSpatialUtility.BoundsEpsilon
                ? forwardLength / ctx.SpriteBoundsSize.x
                : 1f;
            var sy = ctx.SpriteBoundsSize.y > TargetSelectSpatialUtility.BoundsEpsilon
                ? lateralWidth / ctx.SpriteBoundsSize.y
                : 1f;
            hintTr.localScale = new Vector3(sx, sy, 1f);
        }

        public override void DrawGizmosSelected(in TargetSelectGizmoContext ctx)
        {
            TargetSelectGizmoUtility.DrawRectangleXY(
                ctx.Origin,
                ctx.ForwardUnit,
                forwardLength,
                lateralWidth,
                centerOffsetAlongForward);
        }
    }
}