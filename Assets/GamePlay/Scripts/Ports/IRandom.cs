
public interface IRandom
{
    uint NextUInt();

    int NextInt(int minInclusive, int maxExclusive);

    float NextFloat01();

    float NextFloat(float minInclusive, float maxInclusive);

    IRandom Fork(ulong salt);
}