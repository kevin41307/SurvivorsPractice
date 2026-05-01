namespace SpatialHash2D
{
    /// <summary>
    /// Intent:
    /// 將「註冊表 + 空間雜湊格」組成單一服務，由 DI 注入；每幀 Rebuild 為目前 MVP 最簡策略。
    /// </summary>
    public sealed class SpatialHashWorld
    {
        private readonly AgentRegistry registry;
        private readonly SpatialHashGrid2D grid;

        public SpatialHashWorld()
            : this(cellSize: 4f)
        {
        }

        public SpatialHashWorld(float cellSize)
        {
            registry = new AgentRegistry();
            grid = new SpatialHashGrid2D(cellSize);
        }

        public int FrameCount { get; private set; }
        public AgentRegistry Registry => registry;
        public SpatialHashGrid2D Grid => grid;

        public void Tick()
        {
            FrameCount++;
            grid.Rebuild(registry.Agents);
        }
    }
}
