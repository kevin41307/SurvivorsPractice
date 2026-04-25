using GamePlay.Scripts.Service.Config;



namespace GamePlay.Scripts.Service
{
    public class TreasureChestRegistry
    {
        private readonly TreasureChestRegistryDefinition treasureChestRegistryDefinition;

        public TreasureChestRegistry(TreasureChestRegistryDefinition treasureChestRegistryDefinition)
        {
            this.treasureChestRegistryDefinition = treasureChestRegistryDefinition;
        }
    }
}