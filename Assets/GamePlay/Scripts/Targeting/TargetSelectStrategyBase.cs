using System;
using System.Collections.Generic;
using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Targeting
{
    public struct TargetSelectHintApplyContext
    {
        public SpriteRenderer HintRenderer;
        public Sprite HintSprite;
        public bool IsHintVisible;
        public Material CircleConeHintMaterial;
        public Material RectangleHintMaterial;
        public Color HintColor;
        public int HintSortingOrder;
        public MaterialPropertyBlock PropertyBlock;
        public int HalfAngleShaderPropertyId;
    }

    public struct TargetSelectHintTransformContext
    {
        public Vector3 CasterPosition;
        public Vector2 PlanarForwardUnit;
        public Transform HintTransform;
        public Vector3 SpriteBoundsSize;
        public float HintZOffset;
    }

    public struct TargetSelectGizmoContext
    {
        public Vector3 Origin;
        public Vector2 ForwardUnit;
    }
    
    
    
    /// <summary>
    /// 目標選取策略（選取、Hint、Gizmo）。以 <see cref="SerializeReference"/> 序列化具體子類別。
    /// </summary>
    [Serializable]
    public abstract class TargetSelectStrategyBase
    {
        public abstract void SelectAllInShape(
            SpatialHashWorld world,
            Vector2 origin,
            Vector2 forward,
            List<ITargetable> results);

        public abstract void ApplyHintVisuals(TargetSelectHintApplyContext ctx);

        public abstract void UpdateHintTransform(in TargetSelectHintTransformContext ctx);

        public abstract void DrawGizmosSelected(in TargetSelectGizmoContext ctx);
    }
}