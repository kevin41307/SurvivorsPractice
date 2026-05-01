using SpatialHash2D;
using UnityEngine;

namespace GamePlay.Scripts.Movement.Ports
{
    /// <summary>
    /// 單一移動管線步驟；多步驟由 <see cref="GamePlay.Scripts.Movement.Steps.MoveStepPipeline"/> 組合。
    /// </summary>
    public interface IMoveStep
    {
        Vector2 GetDisplacement(in MoveContext context, GridAgent self);
    }
}
