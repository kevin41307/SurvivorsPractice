using UnityEngine;
using System;

namespace GamePlay.Scripts.Movement.Ports
{
    [Serializable]
    public abstract class BaseMovePolicy
    {
        public int bx;
        private int by;
        public int bz { get; set; }

    }
    
    [Serializable]
    public class DebugMovePolicy : BaseMovePolicy
    {
        public int x;
        private int y;

        public Vector2 position;
    }
}