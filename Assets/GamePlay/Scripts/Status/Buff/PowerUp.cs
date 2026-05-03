using GamePlay.Scripts.MetaProgress.Config;

namespace GamePlay.Scripts.Status.Buff
{
    public class PowerUp
    {
        public MetaPowerUpDefinition Definition { get; }

        public int Level { get; private set; }

        public PowerUp(MetaPowerUpDefinition definition, int level)
        {
            Definition = definition;
            Level = level;
        }
        
        
        public float GetTotalBonus()
        {
            return Definition.GetTotalBonus(Level);
        }        

    }
}