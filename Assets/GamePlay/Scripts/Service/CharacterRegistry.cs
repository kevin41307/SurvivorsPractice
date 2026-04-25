using GamePlay.Scripts.Service.Config;

namespace GamePlay.Scripts.Service
{
    public class CharacterRegistry
    {
        private readonly CharacterRegistryDefinition characterRegistryDefinition;
          
        public CharacterRegistry(CharacterRegistryDefinition characterRegistryDefinition)
        {
            this.characterRegistryDefinition = characterRegistryDefinition;
        }


    }
}