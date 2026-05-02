using System.Collections.Generic;
using GamePlay.Scripts.Application.DI;
using GamePlay.Scripts.Combat.Ports;
using SpatialHash2D;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Targeting
{
    /// <summary>
    /// Play 模式下驗收 TargetSelectView 的提示顯示：按住指定鍵或滑鼠左鍵時 Show + 每幀 Update，放開則 Hide。
    /// 掛在與 <see cref="TargetSelectView"/> 同一物件即可（未指定時會自動 GetComponent）。
    /// </summary>
    public sealed class TargetSelectDebugDriver : MonoBehaviour
    {
        [SerializeField]
        TargetSelectView targetSelect;

        [SerializeField]
        bool useMouseLeftButton = true;

        [SerializeField]
        bool useSpaceKey = true;

        [Tooltip("未指定時會嘗試 FindFirstObjectByType<Mvp1Installer> 以解析 SpatialHashWorld。")]
        [SerializeField]
        LifetimeScope lifetimeScope;

        bool wasShowing;
        SpatialHashWorld spatialHashWorld;
        readonly List<ITargetable> selectionBuffer = new List<ITargetable>();


        void Awake()
        {
            if (targetSelect == null)
            {
                targetSelect = GetComponent<TargetSelectView>();
            }
        }

        void Start()
        {
            var scope = lifetimeScope != null ? lifetimeScope : FindFirstObjectByType<Mvp1Installer>();
            if (scope != null && scope.Container != null)
            {
                spatialHashWorld = scope.Container.Resolve<SpatialHashWorld>();
            }
        }

        void Update()
        {
            if (targetSelect == null)
            {
                return;
            }

            if (spatialHashWorld != null && WasSelectButtonPressedThisFrame())
            {
                LogSelectedTargets();
            }

            var show = IsHoldActive();
            if (show)
            {
                if (!wasShowing)
                {
                    targetSelect.ShowHint();
                    wasShowing = true;
                }

                targetSelect.UpdateHint();
            }
            else
            {
                if (wasShowing)
                {
                    targetSelect.HideHint();
                    wasShowing = false;
                }
            }
        }

        bool IsHoldActive()
        {
            if (useMouseLeftButton && Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                return true;
            }

            if (useSpaceKey && Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            {
                return true;
            }

            return false;
        }

        bool WasSelectButtonPressedThisFrame()
        {
            if (useMouseLeftButton && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                return true;
            }

            if (useSpaceKey && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                return true;
            }

            return false;
        }

        void LogSelectedTargets()
        {
            targetSelect.SelectAllInShape(spatialHashWorld, selectionBuffer);
            if (selectionBuffer.Count == 0)
            {
                Debug.Log("[TargetSelectDebugDriver] 選中：（無）", this);
                return;
            }

            for (var i = 0; i < selectionBuffer.Count; i++)
            {
                var t = selectionBuffer[i];
                Debug.Log($"[TargetSelectDebugDriver] 選中 [{i}] {t.Transform.name}", t.Transform);
                if (t is ICombatable combatable)
                {
                    combatable.TakeDamage(1f);
                }
            }
        }
    }
}
