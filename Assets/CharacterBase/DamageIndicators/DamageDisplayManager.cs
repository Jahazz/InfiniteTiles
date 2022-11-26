
using InfiniteTiles.Character.PlayerCharacter;
using UnityEngine;

public class DamageDisplayManager : MonoBehaviour
{
    [field: SerializeField]
    private DamageParticle DamageParticlePrefab { get; set; }
    [field: SerializeField]
    private Transform SpawnedElementParent { get; set; }
    [field: SerializeField]
    private InfiniteTiles.Character.PlayerCharacter.CharacterController MainCharacter { get; set; }
    [field: SerializeField]
    private EnemyManager MainEnemyManager { get; set; }
    [field: SerializeField]
    private float YSpawnOffset { get; set; }

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
        MainEnemyManager.OnEnemySpawned += HandleOnEnemySpawned;
        MainCharacter.OnHitRecieved += HandleHitRecieved;
        MainCharacter.OnCharacterDeath += () => HandleOnCharacterDeath(MainCharacter);
    }

    private void DetachFromEvents ()
    {
        MainEnemyManager.OnEnemySpawned -= HandleOnEnemySpawned;
        MainCharacter.OnHitRecieved -= HandleHitRecieved;
    }

    private void HandleOnCharacterDeath (IBaseCharacter dyingCharacter)
    {
        dyingCharacter.OnCharacterDeath -= () => HandleOnCharacterDeath(dyingCharacter);
        dyingCharacter.OnHitRecieved -= HandleHitRecieved;
    }

    private void HandleOnEnemySpawned (Enemy spawnedEnemy)
    {
        spawnedEnemy.OnHitRecieved += HandleHitRecieved;
        spawnedEnemy.OnCharacterDeath += () => HandleOnCharacterDeath(spawnedEnemy);
    }

    private void HandleHitRecieved (IBaseCharacter target, int hitValue, bool isCrit)
    {
        Vector3 spawnLocation = target.ConnectedRigidbody.transform.position;
        spawnLocation.y += YSpawnOffset;

        SpawnDamage(spawnLocation, hitValue, isCrit);
    }

    public void SpawnDamage (Vector3 spawnLocation, int damageValue, bool isCritical)
    {
        Instantiate(DamageParticlePrefab, spawnLocation, Quaternion.identity, SpawnedElementParent).Initialize(damageValue, isCritical);
    }
}
