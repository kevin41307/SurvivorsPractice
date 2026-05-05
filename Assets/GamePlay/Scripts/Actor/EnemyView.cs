using DG.Tweening;
using UnityEngine;
using GamePlay.Scripts.Actor.Config;
using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Targeting;
using VContainer;

namespace GamePlay.Scripts.Actor
{
    [RequireComponent(typeof(EnemyMovementController))]
    public class EnemyView : MonoBehaviour, ITargetable, ICombatable
    {
        [Inject]
        public Enemy Enemy { get; set; }

        internal int PoolKey { get; set; }
        public EnemyViewDefinition ViewDefinition { get; private set; }

        SpriteRenderer _spriteRenderer;
        Color _originalColor;
        EnemyMovementController movementController;

        void Awake()
        {
            movementController = GetComponent<EnemyMovementController>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer != null)
                _originalColor = _spriteRenderer.color;
        }

        public void Initialize(int poolKey, EnemyViewDefinition viewDefinition)
        {
            PoolKey = poolKey;
            ViewDefinition = viewDefinition;
        }

        Transform ITargetable.Transform => transform;
        Vector3 ICombatable.Position => transform.position;
        
        public void TakeDamage(float amount)
        {
            Debug.Log($"EnemyView TakeDamage: {amount}");
            if (_spriteRenderer == null)
                return;

            _spriteRenderer.DOKill();
            _spriteRenderer.color = Color.red;
            _spriteRenderer.DOColor(_originalColor, 0.3f);
        }

        public void ApplyKnockback(Vector2 directionUnit, float dealtProduct)
        {
            movementController?.ApplyKnockback(directionUnit, dealtProduct);
        }
    }
}
