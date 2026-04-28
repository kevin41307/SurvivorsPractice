namespace GamePlay.Scripts.Core.Ports
{
    public interface ITimeProvider
    {
        float Time { get; }
        float DeltaTime { get; }
        int FrameCount { get; }
    }
}