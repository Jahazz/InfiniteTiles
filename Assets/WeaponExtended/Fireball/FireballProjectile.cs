using InfiniteTiles.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Weapon.Fireball
{
    public class FireballProjectile : MonoBehaviour
    {
        private Action<IDamageable> OnHitCallback { get; set; }
        private IDamageable DefaultTarget { get; set; }
        private Vector3 InitialTargetPosition { get; set; }
        [field: SerializeField]
        private ParticleSystemOneShooter ExplosionEffect { get; set; }
        [field: SerializeField]
        private Rigidbody ConnectedRigidbody { get; set; }

        private FirebalCaster CastingCaster { get; set; }
        private bool IsAlive { get; set; } = true;

        const string ENEMY_LAYER_NAME = "Enemy";

        public void Initialize (FirebalCaster caster, Action<IDamageable> onHitCallback, IDamageable defaultTarget)
        {
            OnHitCallback = onHitCallback;
            DefaultTarget = defaultTarget;
            InitialTargetPosition = DefaultTarget.GetTargetTransform().position;
            ConnectedRigidbody.AddForce((InitialTargetPosition - caster.transform.position).normalized * 10f, ForceMode.VelocityChange);
        }

        protected virtual void OnCollisionEnter (Collision collision)
        {
            if (IsAlive == true)
            {
                ConnectedRigidbody.isKinematic = true;
                ConnectedRigidbody.velocity = Vector3.zero;

                IsAlive = false;
                Instantiate(ExplosionEffect, collision.contacts[0].point, Quaternion.identity, collision.transform).PlayOneShot();

                if (collision.gameObject.layer == LayerMask.NameToLayer(ENEMY_LAYER_NAME))
                {
                    OnHitCallback(collision.gameObject.GetComponent<Enemy>());
                }

                Destroy(gameObject, 3.0f);
            }

        }

    }
}
