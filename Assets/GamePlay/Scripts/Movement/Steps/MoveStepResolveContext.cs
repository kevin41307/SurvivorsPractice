using SpatialHash2D;

namespace GamePlay.Scripts.Movement.Steps
{
    public readonly struct MoveStepResolveContext
    {
        public MoveStepResolveContext(SpatialHashWorld spatialHashWorld)
        {
            SpatialHashWorld = spatialHashWorld;
        }

        public SpatialHashWorld SpatialHashWorld { get; }
    }
}
