using System;
using GamePlay.Scripts.Actor;
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
    public sealed class CharacterFactory
    {
        private readonly IObjectResolver resolver;

        public CharacterFactory(IObjectResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public CharacterView Create(CharacterViewDefinition definition, Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            
            if ( definition.prefab == null)
            {
                throw new Exception($"[CharacterFactory] CharacterDefinition '{definition.name}' 的 viewDefinition/prefab 未設定。");
            }

            GameObject go = parent == null
                ? resolver.Instantiate(definition.prefab)
                : resolver.Instantiate(definition.prefab, parent);

            if (!go.TryGetComponent(out CharacterView view) || view == null)
            {
                throw new Exception($"[CharacterFactory] Prefab '{definition.prefab.name}' 未找到 CharacterView 元件。");
            }

            view.Character = new Character();


            return view;
        }
    }
}
