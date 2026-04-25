using System;
using System.Collections.Generic;

public sealed class RandomProvider : IRandomProvider
{
    private readonly ulong _runSeed;
    private readonly Dictionary<RandomStream, IRandom> _streams = new();

    public RandomProvider(ulong runSeed)
    {
        _runSeed = runSeed;
    }

    public IRandom Get(RandomStream stream)
    {
        if (_streams.TryGetValue(stream, out var rng))
            return rng;

        rng = CreateStream(stream);
        _streams.Add(stream, rng);
        return rng;
    }

    public IRandom Fork(RandomStream stream, ulong salt)
    {
        var seed = DeriveSeed(_runSeed, (ulong)stream, salt);
        var streamId = DeriveSeed(_runSeed ^ 0xD1B54A32D192ED03UL, (ulong)stream, salt ^ 0x9E3779B97F4A7C15UL);
        return new Pcg32Random(seed, streamId);
    }

    private IRandom CreateStream(RandomStream stream)
    {
        var seed = DeriveSeed(_runSeed, (ulong)stream, 0UL);
        var streamId = DeriveSeed(_runSeed ^ 0xA24BAED4963EE407UL, (ulong)stream, 1UL);
        return new Pcg32Random(seed, streamId);
    }

    private static ulong DeriveSeed(ulong runSeed, ulong stream, ulong salt)
    {
        // Mix three inputs into a single 64-bit seed deterministically.
        ulong x = runSeed;
        x = SplitMix64.Mix(x ^ (stream * 0x9E3779B97F4A7C15UL));
        x = SplitMix64.Mix(x ^ (salt * 0xBF58476D1CE4E5B9UL));
        return x;
    }
}