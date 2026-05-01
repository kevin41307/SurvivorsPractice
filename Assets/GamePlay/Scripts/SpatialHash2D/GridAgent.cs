using System;
using UnityEngine;

namespace SpatialHash2D
{
    /// <summary>
    /// Intent:
    /// 以 Transform 為資料來源的 2D 空間索引實體（與遊戲角色種類無關）。
    /// </summary>
    public sealed class GridAgent
    {
        public GridAgent(int id, Transform transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            Id = id;
            Transform = transform;
        }

        public int Id { get; }
        public Transform Transform { get; }

        public Vector2 Position2D
        {
            get
            {
                var p = Transform.position;
                return new Vector2(p.x, p.y);
            }
        }
    }
}
