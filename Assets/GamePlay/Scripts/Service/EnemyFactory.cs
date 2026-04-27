using System;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GamePlay.Scripts.Service
{
    /// <summary>
    /// 參考 VContainer 常見 Factory/Func 注入模式：
    /// - 由工廠負責從 Definition 產生 View + Domain 物件並完成初始化
    /// - Instantiate 走 IObjectResolver，確保 prefab 上可注入相依
    /// </summary>
    public sealed class EnemyFactory
    {
        private readonly IObjectResolver resolver;

        public EnemyFactory(IObjectResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public EnemyView Create(EnemyViewDefinition definition, Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            
            if (definition.prefab == null)
            {
                throw new Exception($"[EnemyFactory] EnemyDefinition '{definition.name}' 的 viewDefinition/prefab 未設定。");
            }

            GameObject go = parent == null
                ? resolver.Instantiate(definition.prefab)
                : resolver.Instantiate(definition.prefab, parent);

            if (!go.TryGetComponent(out EnemyView view) || view == null)
            {
                throw new Exception($"[EnemyFactory] Prefab '{definition.prefab.name}' 未找到 EnemyView 元件。");
            }

            return view;
        }
    }
}
