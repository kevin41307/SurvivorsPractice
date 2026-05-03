namespace GamePlay.Scripts.Combat.Ports
{
    /// <summary>
    /// 可選實作：目標若回傳 true，傷害管線會在 Invulnerability 節點標記取消，後續不再扣血。
    /// </summary>
    public interface IInvulnerable
    {
        bool IsInvulnerable { get; }
    }
}
