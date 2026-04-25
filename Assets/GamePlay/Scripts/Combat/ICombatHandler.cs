namespace GamePlay.Scripts.Combat.Ports
{
    public interface ICombatHandler
    {
        void Handle(ref CombatContext context);

    }
}