using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash2D
{
    /// <summary>
    /// Intent:
    /// 2D 空間雜湊格：以 cell 分桶，鄰居查詢僅掃描 3×3 格（與專案業務無關，可複製到其他 Unity 專案）。
    /// </summary>
    public sealed class SpatialHashGrid2D
    {
        private readonly Dictionary<int, List<GridAgent>> buckets = new();
        private readonly List<GridAgent> queryBuffer = new(64);

        public SpatialHashGrid2D(float cellSize)
        {
            CellSize = Mathf.Max(0.01f, cellSize);
        }

        public float CellSize { get; }

        public void Clear()
        {
            foreach (var kv in buckets)
            {
                kv.Value?.Clear();
            }

            buckets.Clear();
        }

        public void Rebuild(IReadOnlyList<GridAgent> agents)
        {
            if (agents == null)
            {
                throw new ArgumentNullException(nameof(agents));
            }

            Clear();

            for (var i = 0; i < agents.Count; i++)
            {
                var agent = agents[i];
                if (agent == null || agent.Transform == null)
                {
                    continue;
                }

                var pos = agent.Position2D;
                var cell = WorldToCell(pos);
                var key = Hash(cell.x, cell.y);

                if (!buckets.TryGetValue(key, out var list) || list == null)
                {
                    list = new List<GridAgent>(16);
                    buckets[key] = list;
                }

                list.Add(agent);
            }
        }

        public Vector2Int WorldToCell(Vector2 worldPos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPos.x / CellSize),
                Mathf.FloorToInt(worldPos.y / CellSize));
        }

        public List<GridAgent> QueryNeighbors(Vector2 worldPos)
        {
            queryBuffer.Clear();

            var center = WorldToCell(worldPos);
            for (var dy = -1; dy <= 1; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    var key = Hash(center.x + dx, center.y + dy);
                    if (!buckets.TryGetValue(key, out var list) || list == null || list.Count == 0)
                    {
                        continue;
                    }

                    queryBuffer.AddRange(list);
                }
            }

            return queryBuffer;
        }

        public static float RadiusToSqr(float radius)
        {
            return radius * radius;
        }

        public static bool IsWithinRadiusSqr(Vector2 a, Vector2 b, float radiusSqr)
        {
            var d = a - b;
            return d.sqrMagnitude < radiusSqr;
        }

        private static int Hash(int cellX, int cellY)
        {
            unchecked
            {
                return (cellX * 73856093) ^ (cellY * 19349663);
            }
        }
    }
}
