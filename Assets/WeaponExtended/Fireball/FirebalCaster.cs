using InfiniteTiles.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Weapon.Fireball
{
    public class FirebalCaster : BaseWeapon<BaseWeaponStats<BaseWeaponData>, BaseWeaponData>
    {
        [field: SerializeField]
        private EnemyManager EnemyManager { get; set; }
        [field: SerializeField]
        private Transform PlayerCharacter { get; set; }
        [field: SerializeField]
        private FireballProjectile Projectile { get; set; }
        [field: SerializeField]
        private ParticleSystemOneShooter CastEffect { get; set; }

        // Update is called once per frame
        protected override void Update ()
        {
            SetTarget();
            base.Update();
        }
        public override void DealDamage (IDamageable target)
        {
            if (IsTargetInRange(target))
            {
                Vector3 spawnPosition = transform.position;
                Instantiate(CastEffect, spawnPosition, Quaternion.identity, transform).PlayOneShot();
                StartCoroutine(CreateProjectile(target));
            }
        }

        public void BaseDealDamage (IDamageable target)
        {
            base.DealDamage(target);
        }

        private IEnumerator CreateProjectile (IDamageable target)
        {
            yield return new WaitForSeconds(1.0f);
            Instantiate(Projectile, transform.position, Quaternion.identity).Initialize(this, BaseDealDamage, target);
        }

        private void SetTarget ()
        {
            Enemy nearestTarget = FindNearestTarget();

            if (Vector3.Distance(nearestTarget.transform.position, PlayerCharacter.position) <= WeaponStats.Range.PresentValue)
            {
                CurrentTarget = nearestTarget;
            }
        }

        private Enemy FindNearestTarget ()
        {
            float closestDistance = -1;
            Enemy closestEnemy = null;

            foreach (Enemy enemy in EnemyManager.CurrentlyPresentEnemies)
            {
                float distanceToCurrentEnemy = Vector3.Distance(enemy.transform.position, PlayerCharacter.position);

                if (closestDistance == -1 || distanceToCurrentEnemy < closestDistance)
                {
                    closestDistance = distanceToCurrentEnemy;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
    }
}
