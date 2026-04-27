using System;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Item.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Service
{
    public sealed class ExpGemFactory
    {
        private readonly IObjectResolver resolver;

        public ExpGemFactory(IObjectResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public ExpGemView Create(ExperienceGemViewDefinition definition, Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition.prefab == null)
            {
                throw new Exception($"[ExpGemFactory] ViewDefinition '{definition.name}' 的 prefab 未設定。");
            }

            GameObject go = parent == null
                ? resolver.Instantiate(definition.prefab)
                : resolver.Instantiate(definition.prefab, parent);

            if (!go.TryGetComponent(out ExpGemView view) || view == null)
            {
                throw new Exception($"[ExpGemFactory] Prefab '{definition.prefab.name}' 未找到 ExpGemView 元件。");
            }

            return view;
        }
    }
}

