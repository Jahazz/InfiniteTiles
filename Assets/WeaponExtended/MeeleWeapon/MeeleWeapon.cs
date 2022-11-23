using InfiniteTiles.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Weapon.MeeleWeapon
{
    public class MeeleWeapon : BaseWeapon<BaseWeaponStats<BaseWeaponData>, BaseWeaponData>
    {
        public override void DealDamage (IDamageable target)
        {
            if (IsTargetInRange(target))
            {
                base.DealDamage(target);
            }
        }
    }
}

