using System;
using GamePlay.Scripts.Core.Ports;

namespace GamePlay.Scripts.Core
{
    public sealed class Pcg32Random : IRandom
    {
        // PCG32 (XSH RR) as per pcg-random.org reference implementation.
        private ulong _state;
        private ulong _inc;

        public Pcg32Random(ulong seed, ulong stream = 0)
        {
            _state = 0UL;
            _inc = (stream << 1) | 1UL;
            NextUInt();
            _state += seed;
            NextUInt();
        }

        public uint NextUInt()
        {
            ulong oldState = _state;
            _state = unchecked(oldState * 6364136223846793005UL + _inc);

            uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), "maxExclusive must be > minInclusive");

            uint range = (uint)(maxExclusive - minInclusive);
            uint threshold = (uint)(-range) % range;

            while (true)
            {
                uint r = NextUInt();
                if (r >= threshold)
                    return (int)(minInclusive + (r % range));
            }
        }

        public float NextFloat01()
        {
            // 24-bit precision float in [0,1)
            return (NextUInt() >> 8) * (1.0f / 16777216.0f);
        }

        public float NextFloat(float minInclusive, float maxInclusive)
        {
            if (maxInclusive < minInclusive)
                throw new ArgumentOutOfRangeException(nameof(maxInclusive), "maxInclusive must be >= minInclusive");

            if (maxInclusive == minInclusive)
                return minInclusive;

            return minInclusive + (maxInclusive - minInclusive) * NextFloat01();
        }

        public IRandom Fork(ulong salt)
        {
            // Forking should be deterministic and not advance the current stream.
            ulong seed = SplitMix64.Mix(_state ^ salt);
            ulong stream = SplitMix64.Mix(_inc + (salt * 0x9E3779B97F4A7C15UL));
            return new Pcg32Random(seed, stream);
        }
    }
}