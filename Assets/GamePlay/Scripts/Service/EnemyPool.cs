using System;
using System.Collections.Generic;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using UnityEngine;
using UnityEngine.Pool;

namespace GamePlay.Scripts.Service
{
    public sealed class EnemyPool
    {
        private const int DefaultCapacity = 10;
        private const int MaxSize = 200;

        private readonly EnemyFactory enemyFactory;
        private readonly Dictionary<int, ObjectPool<EnemyView>> poolsByPrefabId = new();

        public EnemyPool(EnemyFactory enemyFactory)
        {
            this.enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
        }

        public EnemyView Get(EnemyViewDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            
            if (definition.prefab == null)
            {
                throw new Exception($"[EnemyPool] EnemyDefinition '{definition.name}' 的 viewDefinition/prefab 未設定。");
            }

            var key = definition.prefab.GetInstanceID();
            var pool = GetOrCreatePool(key, definition);

            var view = pool.Get();
            view.Initialize(key);
            return view;
        }

        public void Release(EnemyView view)
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

        private ObjectPool<EnemyView> GetOrCreatePool(int prefabId, EnemyViewDefinition definition)
        {
            if (poolsByPrefabId.TryGetValue(prefabId, out var existing) && existing != null)
            {
                return existing;
            }

            ObjectPool<EnemyView> pool = null;

            EnemyView Create()
            {
                var view = enemyFactory.Create(definition);
                view.Initialize(prefabId);
                return view;
            }

            void OnGet(EnemyView view)
            {
                if (view != null)
                {
                    view.gameObject.SetActive(true);
                }
            }

            void OnRelease(EnemyView view)
            {
                if (view != null)
                {
                    view.gameObject.SetActive(false);
                }
            }

            void OnDestroy(EnemyView view)
            {
                if (view != null)
                {
                    UnityEngine.Object.Destroy(view.gameObject);
                }
            }

            pool = new ObjectPool<EnemyView>(
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
