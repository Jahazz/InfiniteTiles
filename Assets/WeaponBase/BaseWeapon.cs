using InfiniteTiles.Character;
using UnityEngine;

namespace InfiniteTiles.Weapon
{
    public class BaseWeapon<BaseWeaponStatsType, BaseWeaponDataType> : MonoBehaviour
        where BaseWeaponDataType : BaseWeaponData
        where BaseWeaponStatsType : BaseWeaponStats<BaseWeaponDataType>, new()
    {
        [field: SerializeField]
        private BaseWeaponDataType WeaponData { get; set; }

        public IDamageable WeaponTarget { get; set; }
        public BaseWeaponStatsType WeaponStats { get; set; }
        private float LastWeaponUsage { get; set; }

        protected virtual void Update ()
        {
            CheckFireConditions();
        }

        public void Initialize ()
        {
            WeaponStats = new BaseWeaponStatsType();
            WeaponStats.InitializeBaseData(WeaponData);
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
            WeaponTarget.GetDamaged(WeaponStats.Damage.PresentValue);
        }

        private bool IsTargetInRange ()
        {
            return WeaponTarget.CalculateRangeBetweenTarget(transform.position) <= WeaponStats.Range.PresentValue;
        }

        private bool IsWeaponOnCooldown ()
        {
            return LastWeaponUsage + WeaponStats.AttackSpeed.PresentValue < Time.time;
        }

        
    }
}

