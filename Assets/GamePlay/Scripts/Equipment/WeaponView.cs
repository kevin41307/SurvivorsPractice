using System.Collections.Generic;
using GamePlay.Scripts.Combat;
using GamePlay.Scripts.Combat.Ports;
using GamePlay.Scripts.Core;
using GamePlay.Scripts.Equipment.Config;
using GamePlay.Scripts.Targeting;
using SpatialHash2D;
using UnityEngine;
using VContainer;

namespace GamePlay.Scripts.Equipment
{
    /// <summary>
    /// 武器表現層：每幀驅動 Domain Weapon 計時，冷卻好就以 TargetSelectView 選目標並經 <see cref="CombatPipeline"/> 結算傷害。
    /// </summary>
    /// <remarks>
    /// Invariant: <see cref="TargetSelectView"/>如果為空, 則不選擇目標, 但還是會執行攻擊. 為武器 prefab 之 child 上的元件並由此序列化引用（見企劃 <c>vampire_survivors_outline</c> 武器章節「實作約束」）。
    /// </remarks>
    public class WeaponView : MonoBehaviour
    {
        public Weapon Weapon { get; private set; }

        private SpatialHashWorld spatialHashWorld;
        private CombatPipeline combatPipeline;
        
        [SerializeField] private TargetSelectView targetSelectView;
        
        readonly List<ITargetable> selectionBuffer = new();

        [Inject]
        public void Construct(Weapon weapon, SpatialHashWorld world, CombatPipeline combatPipeline)
        {
            Weapon = weapon;
            spatialHashWorld = world;
            this.combatPipeline = combatPipeline;
        }

        public void Initialize(WeaponDefinition definition)
        {
            if (Weapon != null)
            {
                Weapon.Configure(definition);
            }
        }

        void Update()
        {
            if (Weapon == null)
            {
                return;
            }

            Weapon.Tick(Time.deltaTime);

            if (Weapon.TryConsumeCooldown())
            {
                Fire();
            }
        }

        void Fire()
        {
            if ( spatialHashWorld == null || combatPipeline == null)
            {
                return;
            }

            Vector2 origin = TargetSelectSpatialUtility.ToPlane(transform.position);
            Vector2 forward = transform.right.ToVector2();

            selectionBuffer.Clear();
            targetSelectView?.SelectAllInShape(spatialHashWorld, origin, forward, selectionBuffer);

            float hitDamage = Weapon.DamageStat.FinalValue;

            for (int i = 0; i < selectionBuffer.Count; i++)
            {
                if (selectionBuffer[i] is not ICombatable combatable)
                {
                    continue;
                }

                var ctx = new CombatContext
                {
                    CasterTransform = transform,
                    Target = combatable,
                    RawDamage = hitDamage,
                    FinalDamage = hitDamage,
                    Cancelled = false,
                    FlatArmorReduction = 0f,
                    ResistanceFraction = 0f,
                };
                combatPipeline.Execute(ref ctx);
            }
        }


        
    }
}
