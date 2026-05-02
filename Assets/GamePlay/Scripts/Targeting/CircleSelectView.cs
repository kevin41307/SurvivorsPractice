using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    /// <summary>
    /// 以 caster 平面位置為圓心、<see cref="Radius"/> 為半徑，選取圓內（含邊界）所有 <see cref="ITargetable"/>。
    /// </summary>
    [Serializable]
    public sealed class CircleSelectView : TargetSelectStrategyBase
    {
        const float ShaderFullDiskHalfAngleDegrees = 180f;

        [MinValue(0.01f)]
        [SerializeField]
        float radius = 5f;

        public CircleSelectView()
        {
        }

        public CircleSelectView(float radiusValue)
        {
            radius = radiusValue;
        }

        public float Radius => radius;

        public override void SelectAllInShape(
            SpatialHashWorld world,
            Vector2 origin,
            Vector2 forward,
            List<ITargetable> results)
        {
            _ = forward;
            if (world == null)
            {
                return;
            }

            var radiusSqr = radius * radius;
            var candidates = world.Grid.QueryAgentsInWorldAabb(
                origin - new Vector2(radius, radius),
                origin + new Vector2(radius, radius));

            foreach (var agent in candidates)
            {
                if (!TargetSelectSpatialUtility.TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var sqr = (TargetSelectSpatialUtility.ToPlane(t.Transform.position) - origin).sqrMagnitude;
                if (sqr > radiusSqr)
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

            if (ctx.CircleConeHintMaterial == null)
            {
                ctx.HintRenderer.enabled = false;
                return;
            }

            ctx.HintRenderer.sharedMaterial = ctx.CircleConeHintMaterial;
            if (ctx.PropertyBlock != null)
            {
                ctx.HintRenderer.GetPropertyBlock(ctx.PropertyBlock);
                ctx.PropertyBlock.SetFloat(ctx.HalfAngleShaderPropertyId, ShaderFullDiskHalfAngleDegrees);
                ctx.HintRenderer.SetPropertyBlock(ctx.PropertyBlock);
            }

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
            hintTr.position = new Vector3(origin3.x, origin3.y, origin3.z + ctx.HintZOffset);
            hintTr.rotation = TargetSelectSpatialUtility.RotationRightToPlanarForward(forward);

            var diameter = radius * 2f;
            var s = ctx.SpriteBoundsSize.x > TargetSelectSpatialUtility.BoundsEpsilon
                ? diameter / ctx.SpriteBoundsSize.x
                : 1f;
            hintTr.localScale = new Vector3(s, s, 1f);
        }

        public override void DrawGizmosSelected(in TargetSelectGizmoContext ctx)
        {
            TargetSelectGizmoUtility.DrawCircleXY(ctx.Origin, radius);
        }
    }
}
