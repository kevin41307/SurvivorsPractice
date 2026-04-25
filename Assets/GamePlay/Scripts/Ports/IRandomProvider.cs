public interface IRandomProvider
{
    /// <summary>
    /// 取得指定 <paramref name="stream"/> 的亂數來源。
    /// 相同 stream 在相同執行條件下應產生一致的序列(若專案採用 deterministic RNG)。
    /// </summary>
    IRandom Get(RandomStream stream);

    /// <summary>
    /// 從指定 <paramref name="stream"/> 衍生(分岔)出新的亂數來源。
    /// <paramref name="salt"/> 用於區分不同子序列；同一個 (stream, salt) 組合在相同條件下應對應到一致的序列。
    /// </summary>
    IRandom Fork(RandomStream stream, ulong salt);
}