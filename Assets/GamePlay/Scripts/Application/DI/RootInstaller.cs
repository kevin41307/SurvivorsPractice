using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootInstaller : LifetimeScope
{
    [SerializeField] private long runSeed = 123;
    
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("RootInstaller");

        // RegisterMessagePipe returns options.
        var options = builder.RegisterMessagePipe(/* configure option */);
        
        // Setup GlobalMessagePipe to enable diagnostics window and global function
        builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
        builder.Register<IEventBus, MessagePipeEventBus>(Lifetime.Singleton);

        builder.Register<IRandomProvider>(_ => new RandomProvider(checked((ulong)runSeed)), Lifetime.Singleton);
        builder.Register<ITimeProvider, UnityTimeProvider>(Lifetime.Singleton);

    }
}
