using AYellowpaper;
using InfiniteTiles.Character;
using UnityEngine;

public class DebugEnemyController : MonoBehaviour
{
    [field: SerializeField]
    private Enemy EnemyToControl { get; set; }
    [field: SerializeField]
    private EnemyManager EnemyManager { get; set; }

    public InterfaceReference<IDamageable> PlayerCharacter;

    protected virtual void Start ()
    {
        EnemyToControl.Initialize(PlayerCharacter.Value, EnemyManager);
    }

    protected virtual void Update ()
    {
        //PlayerCharacter.Value.GetTargetTransform().position = EnemyToControl.transform.position + (Vector3.forward * 10);
    }

}
