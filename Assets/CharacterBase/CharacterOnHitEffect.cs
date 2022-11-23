using AYellowpaper;
using UnityEngine;

public class CharacterOnHitEffect : MonoBehaviour
{
    [field: SerializeField]
    private InterfaceReference<IBaseCharacter> BaseCharacter { get; set; }
    [field: SerializeField]
    private ParticleSystemOneShooter ParticleSystemPrefab { get; set; }

    protected virtual void OnEnable ()
    {
        AttachToEvents();
    }

    protected virtual void OnDisable ()
    {
        DetachFromEvents();
    }

    private void AttachToEvents ()
    {
        BaseCharacter.Value.OnHitRecieved += HandleOnHitRecieved;
    }

    private void DetachFromEvents ()
    {
        BaseCharacter.Value.OnHitRecieved -= HandleOnHitRecieved;
    }

    private void HandleOnHitRecieved ()
    {
        Instantiate(ParticleSystemPrefab, transform).PlayOneShot();
    }
}
