using System.Collections.Generic;
using Kryz.RPG.Stats.Core;
using Kryz.RPG.Stats.Default;

namespace GamePlay.Scripts.Ports
{
    public interface IStatModifierProvider
    {
        public List<StatModifier<StatModifierData>> GetStatModifiers();
    }
}