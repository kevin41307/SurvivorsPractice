using System;
using GamePlay.Scripts.Item;
using GamePlay.Scripts.Item.Config;

namespace GamePlay.Scripts.Service
{
    public sealed class PassiveItemFactory
    {
        public PassiveItem Create(PassiveItemDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            return new PassiveItem(definition);
        }
    }
}

