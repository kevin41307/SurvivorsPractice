using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash2D
{
    /// <summary>
    /// Intent:
    /// 維護目前參與空間索引的 Transform 清單與穩定 Id（供分幀等邏輯使用）。
    /// </summary>
    public sealed class AgentRegistry
    {
        private int nextId = 1;
        private readonly Dictionary<Transform, GridAgent> agentsByTransform = new();
        private readonly List<GridAgent> agents = new();

        public IReadOnlyList<GridAgent> Agents => agents;

        public GridAgent Register(Transform transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            if (agentsByTransform.TryGetValue(transform, out var existing) && existing != null)
            {
                return existing;
            }

            var agent = new GridAgent(nextId++, transform);
            agentsByTransform[transform] = agent;
            agents.Add(agent);
            return agent;
        }

        public void Unregister(Transform transform)
        {
            if (transform == null)
            {
                return;
            }

            if (!agentsByTransform.TryGetValue(transform, out var agent) || agent == null)
            {
                return;
            }

            agentsByTransform.Remove(transform);
            agents.Remove(agent);
        }
    }
}
