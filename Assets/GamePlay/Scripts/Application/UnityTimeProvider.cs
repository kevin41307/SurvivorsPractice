
public sealed class UnityTimeProvider : ITimeProvider
{
    public float Time => UnityEngine.Time.time;
    public float DeltaTime => UnityEngine.Time.deltaTime;
    public int FrameCount => UnityEngine.Time.frameCount;
}