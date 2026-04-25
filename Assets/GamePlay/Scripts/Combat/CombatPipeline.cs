using System.Collections.Generic;
using GamePlay.Scripts.Combat.Ports;

namespace GamePlay.Scripts.Combat
{
    public class CombatPipeline
    {
        private List<ICombatHandler> handlers;

        public CombatPipeline(List<ICombatHandler> handlers)
        {
            this.handlers = handlers;
        }
        
        public void Execute(CombatContext context)
        {
            foreach (var handler in handlers)
            {
                handler.Handle(ref context);
            }
        
        }
        
    }
}