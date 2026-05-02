using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SpatialHash2D;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.Scripts.Targeting
{
    /// <summary>
    /// 掛在施放者 Prefab：提示圖、Runtime hint、Scene Gizmo、選取查詢。幾何為 XY；朝前見 <see cref="GetPlanarForward"/>。
    /// 形狀邏輯由 <see cref="Strategy"/>（<see cref="NearestSelectView"/>／<see cref="CircleSelectView"/>／<see cref="ConeSelectView"/>／<see cref="RectangleSelectView"/>）提供。
    /// </summary>
    public sealed class TargetSelectView : MonoBehaviour
    {
        [Title("企劃／編輯參考")]
        [Title("選取策略")]
        [SerializeReference, SerializeField]
        TargetSelectStrategyBase strategy;

        [PreviewField(Height = 64)]
        [SerializeField]
        Sprite hintSprite;

        [Title("Gizmo")]
        [SerializeField]
        Color gizmoColor = new Color(0.2f, 0.85f, 1f, 0.9f);

        [Title("Hint (Runtime)")]
        [SerializeField]
        bool autoCreateHintRenderer = true;

        SpriteRenderer hintRenderer;

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

        static readonly int HalfAngleId = Shader.PropertyToID("_HalfAngle");

        public TargetSelectStrategyBase Strategy => strategy;

        void Awake()
        {
            EnsureStrategy();
            EnsureHintRenderer();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            EnsureStrategy();
        }
#endif

        void Reset()
        {
            if (strategy == null)
            {
                strategy = new NearestSelectView();
            }
        }

        void EnsureStrategy()
        {
            if (strategy != null)
            {
                return;
            }
            
        }

        public void ShowHint()
        {
            EnsureStrategy();
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

        public void SelectAllInShape(SpatialHashWorld world, List<ITargetable> results) =>
            SelectAllInShape(world, TargetSelectSpatialUtility.ToPlane(transform.position), GetPlanarForward(), results);

        public void SelectAllInShape(SpatialHashWorld world, Vector2 origin, Vector2 forward, List<ITargetable> results)
        {
            EnsureStrategy();
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            results.Clear();
            if (world == null || strategy == null)
            {
                return;
            }

            strategy.SelectAllInShape(world, origin, forward, results);
        }

        public Vector2 GetPlanarForward()
        {
            var forward = new Vector2(transform.right.x, transform.right.y);
            if (forward.sqrMagnitude < TargetSelectSpatialUtility.EpsilonSqr)
            {
                return Vector2.right;
            }

            forward.Normalize();
            return forward;
        }

        void RefreshHint()
        {
            EnsureStrategy();
            if (strategy == null)
            {
                return;
            }

            hintPropertyBlock ??= new MaterialPropertyBlock();
            var applyCtx = new TargetSelectHintApplyContext
            {
                HintRenderer = hintRenderer,
                HintSprite = hintSprite,
                IsHintVisible = isHintVisible,
                CircleConeHintMaterial = circleConeHintMaterial,
                RectangleHintMaterial = rectangleHintMaterial,
                HintColor = hintColor,
                HintSortingOrder = hintSortingOrder,
                PropertyBlock = hintPropertyBlock,
                HalfAngleShaderPropertyId = HalfAngleId,
            };

            strategy.ApplyHintVisuals(applyCtx);

            if (hintRenderer == null || !hintRenderer.enabled || hintSprite == null)
            {
                return;
            }

            var transformCtx = new TargetSelectHintTransformContext
            {
                CasterPosition = transform.position,
                PlanarForwardUnit = GetPlanarForward(),
                HintTransform = hintRenderer.transform,
                SpriteBoundsSize = hintSprite.bounds.size,
                HintZOffset = hintZOffset,
            };

            strategy.UpdateHintTransform(in transformCtx);
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

        void OnDrawGizmosSelected()
        {
            EnsureStrategy();
            if (strategy == null)
            {
                return;
            }

            var forward = GetPlanarForward();
            var prev = Gizmos.color;
            Gizmos.color = gizmoColor;
            strategy.DrawGizmosSelected(
                new TargetSelectGizmoContext
                {
                    Origin = transform.position,
                    ForwardUnit = forward,
                });
            Gizmos.color = prev;
        }
    }
}
