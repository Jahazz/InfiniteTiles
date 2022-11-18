using InfiniteTiles.Character;
using UnityEngine;

namespace InfiniteTiles.Weapon
{
    public class BaseWeapon<BaseWeaponStatsType, BaseWeaponDataType> : MonoBehaviour, ITargetable
        where BaseWeaponDataType : BaseWeaponData
        where BaseWeaponStatsType : BaseWeaponStats<BaseWeaponDataType>, new()
    {
        [field: SerializeField]
        private BaseWeaponDataType WeaponData { get; set; }
        public BaseWeaponStatsType WeaponStats { get; set; }
        private float LastWeaponUsage { get; set; }
        public IDamageable CurrentTarget { get; set; }

        protected virtual void Update ()
        {
            CheckFireConditions();
        }

        protected virtual void Start ()
        {
            Initialize();
        }

        public void Initialize ()
        {
            WeaponStats = new BaseWeaponStatsType();
            WeaponStats.InitializeBaseData(WeaponData);
            LastWeaponUsage = Time.time;
        }

        private void CheckFireConditions ()
        {
            if (IsTargetInRange() == true && IsWeaponOnCooldown() == false)
            {
                LastWeaponUsage = Time.time;
                UseWeapon();
            }
        }

        protected virtual void UseWeapon ()
        {
            CurrentTarget.GetDamaged(WeaponStats.Damage.PresentValue);
        }

        private bool IsTargetInRange ()
        {
            return Vector3.Distance(CurrentTarget.GetTargetTransform().position,transform.position) <= WeaponStats.Range.PresentValue;
        }

        private bool IsWeaponOnCooldown ()
        {
            return LastWeaponUsage + WeaponStats.AttackSpeed.PresentValue > Time.time;
        }

        protected void OnDrawGizmos ()
        {
            Gizmos.DrawWireSphere(transform.position, WeaponStats.Range.PresentValue);
        }
    }
}

