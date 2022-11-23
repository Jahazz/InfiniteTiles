using AYellowpaper;
using InfiniteTiles.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteTiles.Character
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        public InterfaceReference<IBaseCharacter> ConnectedCharacter;
        [field: SerializeField]
        private Animator CharacterAnimator { get; set; }
        [field: SerializeField]
        private string MovementSpeedVariableName { get; set; }
        [field: SerializeField]
        private string DeathVariableName { get; set; }
        [field: SerializeField]
        private List<string> AttackVariableNameCollection { get; set; }

        protected virtual void Update ()
        {
            UpdateAnimationSpeed();
        }

        protected virtual void OnEnable ()
        {
            AttachToEvents();
        }

        protected virtual void OnDisable ()
        {
            DetachFromEvents();
        }

        private void UpdateAnimationSpeed ()
        {
            CharacterAnimator.SetFloat(MovementSpeedVariableName, ConnectedCharacter.Value.ConnectedRigidbody.velocity.magnitude);
        }

        private void AttachToEvents ()
        {
            int weaponIndex = 0;
            foreach (IBaseWeapon weapon in ConnectedCharacter.Value.WeaponsCollection)
            {
                int currentIndex = weaponIndex;
                weapon.OnAttackStart += (target) => HandleWeaponAttackStart(target, weapon, currentIndex);
                weaponIndex++;
            }
        }

        private void HandleWeaponAttackStart (IDamageable target, IBaseWeapon weapon, int weaponIndex)
        {
            CharacterAnimator.SetTrigger(AttackVariableNameCollection[weaponIndex]);
            weapon.DealDamage(target);
        }

        private void HandleWeaponDamageDealAnimationPoint ()
        {

        }

        private void DetachFromEvents ()
        {

        }
    }
}
