using GamePlay.Scripts.Loot;

namespace GamePlay.Scripts.LevelUp.Ports
{
    public interface ILevelUpCommand : IWeightEntry
    {
        void Execute();
        
        
    }
    
    
}

