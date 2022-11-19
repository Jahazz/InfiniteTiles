using AYellowpaper;
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
        private List<string> AttackVariableNameCollection { get; set; }

        protected virtual void Update ()
        {
            UpdateAnimationSpeed();
        }

        private void UpdateAnimationSpeed ()
        {
            CharacterAnimator.SetFloat(MovementSpeedVariableName, ConnectedCharacter.Value.ConnectedRigidbody.velocity.magnitude);
        }
    }
}
