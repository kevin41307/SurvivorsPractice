using System;
using GamePlay.Scripts.Core.Ports;
using MessagePipe;

namespace GamePlay.Scripts.Core
{
    public sealed class MessagePipeEventBus : IEventBus
    {
        public void Publish<T>(T message)
        {
            GlobalMessagePipe.GetPublisher<T>().Publish(message);
        }

        public IDisposable Subscribe<T>(Action<T> handler)
        {
            return GlobalMessagePipe.GetSubscriber<T>().Subscribe(handler);
        }
    }
}