using GamePlay.Scripts.Service.Config;

namespace GamePlay.Scripts.Service
{
    public class EnemyPoolRegistry
    {
        private readonly EnemyPoolRegistryDefinition enemyPoolRegistryDefinition;

        public EnemyPoolRegistry(EnemyPoolRegistryDefinition enemyPoolRegistryDefinition)
        {
            this.enemyPoolRegistryDefinition = enemyPoolRegistryDefinition;
        }   
        
    }
}