using UnityEngine;

namespace GamePlay.Scripts.Core
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);
    }
}