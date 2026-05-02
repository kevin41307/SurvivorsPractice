using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialHash2D
{
    /// <summary>
    /// Intent:
    /// 2D 空間雜湊格：以 cell 分桶；<see cref="QueryNeighbors"/> 掃 3×3 格，
    /// <see cref="QueryAgentsInWorldAabb"/> 掃與 XY 軸對齊 AABB 相交之所有格。
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

        /// <summary>
        /// 回傳落在與 <paramref name="worldMin"/>～<paramref name="worldMax"/>（XY 含括、會自動校正 min/max）
        /// 相交之 cell 內的 agent。每桶 agent 僅存一格，結果不重複。
        /// 回傳為內部重用之 list，下次呼叫任一 Query 方法會清除。
        /// </summary>
        public List<GridAgent> QueryAgentsInWorldAabb(Vector2 worldMin, Vector2 worldMax)
        {
            queryBuffer.Clear();

            var minX = Mathf.Min(worldMin.x, worldMax.x);
            var maxX = Mathf.Max(worldMin.x, worldMax.x);
            var minY = Mathf.Min(worldMin.y, worldMax.y);
            var maxY = Mathf.Max(worldMin.y, worldMax.y);

            var minCell = WorldToCell(new Vector2(minX, minY));
            var maxCell = WorldToCell(new Vector2(maxX, maxY));

            for (var cy = minCell.y; cy <= maxCell.y; cy++)
            {
                for (var cx = minCell.x; cx <= maxCell.x; cx++)
                {
                    var key = Hash(cx, cy);
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
