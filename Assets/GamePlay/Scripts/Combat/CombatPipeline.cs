using System.Collections.Generic;
using GamePlay.Scripts.Combat.Ports;

namespace GamePlay.Scripts.Combat
{
    public class CombatPipeline
    {
        private readonly List<ICombatHandler> handlers;

        public CombatPipeline(List<ICombatHandler> handlers)
        {
            this.handlers = handlers ?? new List<ICombatHandler>();
        }

        public void Execute(ref CombatContext context)
        {
            for (var i = 0; i < handlers.Count; i++)
            {
                handlers[i].Handle(ref context);
            }
        }
    }
}
