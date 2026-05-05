using UnityEngine;

namespace GamePlay.Scripts.Combat.Ports
{
    public interface ICombatable 
    {
        void TakeDamage(float amount);
        
        void ApplyKnockback(Vector2 directionUnit, float dealtProduct);
        
        Vector3 Position { get; }
    }
}