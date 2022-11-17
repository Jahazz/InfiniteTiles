using InfiniteTiles.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : BaseCharacter<BaseCharacterStats<BaseCharacterData>, BaseCharacterData>
{

    [field: SerializeField]
    private Animator CharacterAnimator { get; set; }
    [field: SerializeField]
    private Transform RotationTransform { get; set; }
    [field: SerializeField]
    private float ForceMultiplier { get; set; }
    [field: SerializeField]
    public Rigidbody CharacterRigidbody { get; set; }
    [field: SerializeField]
    public Collider CharacterCollider { get; set; }
    private float CurrentCharacterSpeed { get; set; }

    public const string CHARACTER_SPEED_VARIABLE_NAME = "MovementSpeed";

    protected override void FixedUpdate ()
    {
        base.FixedUpdate();

        UpdateCharacterSpeed();
        UpdateAnimationSpeed();
    }

    private void UpdateCharacterSpeed ()
    {
        CharacterRigidbody.AddForce(CurrentCharacterSpeed * ForceMultiplier * RotationTransform.forward, ForceMode.VelocityChange);
    }

    private void SetSpeed(float speed)
    {
        CurrentCharacterSpeed = speed;
    }

    private void UpdateAnimationSpeed ()
    {
        CharacterAnimator.SetFloat(CHARACTER_SPEED_VARIABLE_NAME, CharacterRigidbody.velocity.magnitude * 0.1f);
    }

    public void HandleMovementInput (InputAction.CallbackContext obj)
    {
        Vector2 value = obj.ReadValue<Vector2>();
        if (value != Vector2.zero)
        {
            RotationTransform.rotation = ConvertInputToRotation(value);
            SetSpeed(CharacterStats.MovementSpeed.CurrentValue.PresentValue);
        }
        else
        {
            SetSpeed(0.0f);
        }
    }

    private Quaternion ConvertInputToRotation (Vector2 input)
    {
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, angle, 0));
    }
}
