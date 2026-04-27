using System;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Item.Config;
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
    public sealed class TreasureChestFactory
    {
        private readonly IObjectResolver resolver;

        public TreasureChestFactory(IObjectResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public TreasureChestView Create(TreasureChestViewDefinition definition, Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition.prefab == null)
            {
                throw new Exception($"[TreasureChestFactory] ViewDefinition '{definition.name}' 的 prefab 未設定。");
            }

            GameObject go = parent == null
                ? resolver.Instantiate(definition.prefab)
                : resolver.Instantiate(definition.prefab, parent);

            if (!go.TryGetComponent(out TreasureChestView view) || view == null)
            {
                throw new Exception($"[TreasureChestFactory] Prefab '{definition.prefab.name}' 未找到 TreasureChestView 元件。");
            }

            view.Initialize();
            return view;
        }
    }
}

