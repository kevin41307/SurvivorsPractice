using System;
using System.Collections.Generic;
using GamePlay.Scripts.Actor;
using GamePlay.Scripts.Actor.Config;
using SpatialHash2D;
using UnityEngine;
using UnityEngine.Pool;

namespace GamePlay.Scripts.Service
{
    public sealed class EnemyPool
    {
        private const int DefaultCapacity = 10;
        private const int MaxSize = 200;

        private readonly EnemyFactory enemyFactory;
        private readonly SpatialHashWorld spatialHashWorld;
        private readonly Dictionary<int, ObjectPool<EnemyView>> poolsByPrefabId = new();

        public EnemyPool(EnemyFactory enemyFactory, SpatialHashWorld spatialHashWorld)
        {
            this.enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            this.spatialHashWorld = spatialHashWorld ?? throw new ArgumentNullException(nameof(spatialHashWorld));
        }

        public EnemyView Get(EnemyViewDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }
            
            if (definition.prefab == null)
            {
                throw new Exception($"[EnemyPool] ViewDefinition '{definition.name}' 的 prefab 未設定。");
            }

            var key = definition.prefab.GetInstanceID();
            var pool = GetOrCreatePool(key, definition);

            var view = pool.Get();
            view.Initialize(key, definition);
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
                view.Initialize(prefabId, definition);
                return view;
            }

            void OnGet(EnemyView view)
            {
                if (view != null)
                {
                    view.gameObject.SetActive(true);
                    spatialHashWorld.Registry.Register(view.transform);
                }
            }

            void OnRelease(EnemyView view)
            {
                if (view != null)
                {
                    spatialHashWorld.Registry.Unregister(view.transform);
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
