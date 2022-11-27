using InfiniteTiles.Character;
using UnityEngine;

namespace InfiniteTiles.Weapon
{
    public class BaseWeapon<BaseWeaponStatsType, BaseWeaponDataType> : MonoBehaviour, IBaseWeapon
        where BaseWeaponDataType : BaseWeaponData
        where BaseWeaponStatsType : BaseWeaponStats<BaseWeaponDataType>, new()
    {
        public event IBaseWeapon.HitEventParameters OnAttackStart;

        [field: SerializeField]
        private BaseWeaponDataType WeaponData { get; set; }

        public IDamageable CurrentTarget { get; set; }
        public BaseWeaponStatsType WeaponStats { get; set; }
        private float LastWeaponUsage { get; set; }
        private IBaseCharacter Owner { get; set; }

        public void Initialize (IBaseCharacter owner)
        {
            WeaponStats = new BaseWeaponStatsType();
            WeaponStats.InitializeBaseData(WeaponData);
            Owner = owner;
            LastWeaponUsage = Time.time;
        }

        public bool IsTargetInRange (IDamageable target)
        {
            return Vector3.Distance(target.GetTargetTransform().position, transform.position) <= WeaponStats.Range.PresentValue;
        }
        public virtual void UseWeapon ()
        {
            OnAttackStart?.Invoke(CurrentTarget);
        }

        public virtual void DealDamage (IDamageable target)
        {
            CurrentTarget.GetDamaged(WeaponStats.Damage.PresentValue);
        }

        protected virtual void Update ()
        {
            CheckFireConditions();
        }

        private void CheckFireConditions ()
        {
            if (Owner.IsAlive == true && CurrentTarget != null && IsTargetInRange(CurrentTarget) == true && IsWeaponOnCooldown() == false)
            {
                LastWeaponUsage = Time.time;
                UseWeapon();
            }
        }

        private bool IsWeaponOnCooldown ()
        {
            return LastWeaponUsage + WeaponStats.AttackSpeed.PresentValue > Time.time;
        }
    }
}

