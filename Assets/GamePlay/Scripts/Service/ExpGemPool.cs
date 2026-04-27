using System;
using System.Collections.Generic;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Item.Config;
using UnityEngine.Pool;

namespace GamePlay.Scripts.Service
{
    public sealed class ExpGemPool
    {
        private const int DefaultCapacity = 10;
        private const int MaxSize = 200;

        private readonly ExpGemFactory expGemFactory;
        private readonly Dictionary<int, ObjectPool<ExpGemView>> poolsByPrefabId = new();

        public ExpGemPool(ExpGemFactory expGemFactory)
        {
            this.expGemFactory = expGemFactory ?? throw new ArgumentNullException(nameof(expGemFactory));
        }

        public ExpGemView Get(ExperienceGemViewDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition.prefab == null)
            {
                throw new Exception($"[ExpGemPool] ViewDefinition '{definition.name}' 的 prefab 未設定。");
            }

            var key = definition.prefab.GetInstanceID();
            var pool = GetOrCreatePool(key, definition);

            var view = pool.Get();
            view.Initialize(key);
            return view;
        }

        public void Release(ExpGemView view)
        {
            if (view == null)
            {
                return;
            }

            var key = view.PoolKey;
            if (!poolsByPrefabId.TryGetValue(key, out var pool) || pool == null)
            {
                UnityEngine.Object.Destroy(view.gameObject);
                return;
            }

            pool.Release(view);
        }

        private ObjectPool<ExpGemView> GetOrCreatePool(int prefabId, ExperienceGemViewDefinition definition)
        {
            if (poolsByPrefabId.TryGetValue(prefabId, out var existing) && existing != null)
            {
                return existing;
            }

            ObjectPool<ExpGemView> pool = null;

            ExpGemView Create()
            {
                var view = expGemFactory.Create(definition);
                view.Initialize(prefabId);
                return view;
            }

            void OnGet(ExpGemView view)
            {
                if (view != null)
                {
                    view.gameObject.SetActive(true);
                }
            }

            void OnRelease(ExpGemView view)
            {
                if (view != null)
                {
                    view.gameObject.SetActive(false);
                }
            }

            void OnDestroy(ExpGemView view)
            {
                if (view != null)
                {
                    UnityEngine.Object.Destroy(view.gameObject);
                }
            }

            pool = new ObjectPool<ExpGemView>(
                createFunc: Create,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroy,
                collectionCheck: true,
                defaultCapacity: DefaultCapacity,
                maxSize: MaxSize);

            poolsByPrefabId[prefabId] = pool;
            return pool;
        }
    }
}

