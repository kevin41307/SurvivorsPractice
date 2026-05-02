using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    [Serializable]
    public sealed class ConeSelectView : TargetSelectStrategyBase
    {
        [MinValue(0.01f)]
        [SerializeField]
        float coneLength = 5f;

        /// <summary>與前向夾角之半角（度），總視野為 2×此值。</summary>
        [Range(0f, 180f)]
        [SerializeField]
        float halfAngleDegrees = 45f;

        public ConeSelectView()
        {
        }

        public ConeSelectView(float coneLengthValue, float halfAngleDegreesValue)
        {
            coneLength = coneLengthValue;
            halfAngleDegrees = halfAngleDegreesValue;
        }

        public float ConeLength => coneLength;
        public float HalfAngleDegrees => halfAngleDegrees;

        public override void SelectAllInShape(
            SpatialHashWorld world,
            Vector2 origin,
            Vector2 forward,
            List<ITargetable> results)
        {
            if (!TargetSelectSpatialUtility.TryNormalizeForward(forward, out var uCone))
            {
                return;
            }

            SelectCone(world, origin, uCone, results);
        }

        void SelectCone(SpatialHashWorld world, Vector2 origin, Vector2 forwardUnit, List<ITargetable> results)
        {
            var lenSqr = coneLength * coneLength;
            var L = coneLength;
            var candidates = world.Grid.QueryAgentsInWorldAabb(
                origin - new Vector2(L, L),
                origin + new Vector2(L, L));

            foreach (var agent in candidates)
            {
                if (!TargetSelectSpatialUtility.TryGetTargetable(agent, out var t))
                {
                    continue;
                }

                var delta = TargetSelectSpatialUtility.ToPlane(t.Transform.position) - origin;
                var distSqr = delta.sqrMagnitude;
                if (distSqr > lenSqr)
                {
                    continue;
                }

                if (distSqr <= TargetSelectSpatialUtility.EpsilonSqr)
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
                ctx.PropertyBlock.SetFloat(ctx.HalfAngleShaderPropertyId, halfAngleDegrees);
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

            var diameter = coneLength * 2f;
            var s = ctx.SpriteBoundsSize.x > TargetSelectSpatialUtility.BoundsEpsilon
                ? diameter / ctx.SpriteBoundsSize.x
                : 1f;
            hintTr.localScale = new Vector3(s, s, 1f);
        }

        public override void DrawGizmosSelected(in TargetSelectGizmoContext ctx)
        {
            TargetSelectGizmoUtility.DrawConeXY(ctx.Origin, ctx.ForwardUnit, coneLength, halfAngleDegrees);
        }
    }
}