using VContainer.Unity;

namespace SpatialHash2D
{
    public sealed class SpatialHashWorldTickEntryPoint : ITickable
    {
        private readonly SpatialHashWorld spatialHashWorld;

        public SpatialHashWorldTickEntryPoint(SpatialHashWorld spatialHashWorld)
        {
            this.spatialHashWorld = spatialHashWorld;
        }

        public void Tick()
        {
            spatialHashWorld.Tick();
        }
    }
}
