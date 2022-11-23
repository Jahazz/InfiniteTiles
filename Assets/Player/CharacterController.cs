using UnityEngine;
using UnityEngine.InputSystem;

namespace InfiniteTiles.Character.PlayerCharacter
{
    public class CharacterController : BaseCharacter<PlayerCharacterStats, PlayerCharacterData>
    {
        [field: SerializeField]
        public Rigidbody CharacterRigidbody { get; set; }
        [field: SerializeField]
        public Collider CharacterCollider { get; set; }

        public void HandleMovementInput (InputAction.CallbackContext obj)
        {
            Vector2 value = obj.ReadValue<Vector2>();

            if (value != Vector2.zero)
            {
                RotationTransform.rotation = ConvertInputToRotation(value);
                CurrentCharacterSpeed = CharacterStats.MovementSpeed.CurrentValue.PresentValue;
            }
            else
            {
                CurrentCharacterSpeed = 0.0f;
            }
        }

        private Quaternion ConvertInputToRotation (Vector2 input)
        {
            float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            return Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }
}
