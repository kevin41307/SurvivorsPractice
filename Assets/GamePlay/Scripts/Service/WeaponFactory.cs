using System;
using GamePlay.Scripts.Equipment;
using GamePlay.Scripts.Equipment.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Service
{
    /// <summary>
    /// 參考 VContainer 常見 Factory/Func 注入模式：
    /// - 由工廠負責從 Definition 產生 View + Domain 物件並完成綁定
    /// - Instantiate 走 IObjectResolver，確保 prefab 上可注入相依
    /// </summary>
    public sealed class WeaponFactory
    {
        private readonly IObjectResolver resolver;

        public WeaponFactory(IObjectResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public WeaponView Create(WeaponViewDefinition definition, Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition.prefab == null)
            {
                throw new Exception($"[WeaponFactory] ViewDefinition '{definition.name}' 的 prefab 未設定。");
            }

            GameObject go = parent == null
                ? resolver.Instantiate(definition.prefab)
                : resolver.Instantiate(definition.prefab, parent);

            if (!go.TryGetComponent(out WeaponView view) || view == null)
            {
                throw new Exception($"[WeaponFactory] Prefab '{definition.prefab.name}' 未找到 WeaponView 元件。");
            }

            view.Initialize();
            return view;
        }
    }
}

