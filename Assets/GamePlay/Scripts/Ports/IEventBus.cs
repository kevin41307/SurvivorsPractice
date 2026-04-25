using System;

public interface IEventBus
{
    void Publish<T>(T message);
    IDisposable Subscribe<T>(Action<T> handler);
}