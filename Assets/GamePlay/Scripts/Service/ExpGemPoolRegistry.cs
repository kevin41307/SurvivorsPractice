using GamePlay.Scripts.Service.Config;

namespace GamePlay.Scripts.Service
{
    public class ExpGemPoolRegistry
    {
        private readonly ExpGemPoolRegistryDefinition expGemPoolRegistryDefinition;

        public ExpGemPoolRegistry(ExpGemPoolRegistryDefinition expGemPoolRegistryDefinition)
        {
            this.expGemPoolRegistryDefinition = expGemPoolRegistryDefinition;
        }   
    }
}