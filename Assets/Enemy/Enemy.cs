using InfiniteTiles.Character;
using UnityEngine;

public class Enemy : BaseCharacter<BaseCharacterStats<BaseCharacterData>, BaseCharacterData>
{
    [field: Space]
    [field: Header(nameof(Enemy))]
    [field: SerializeField]
    private Rigidbody ConnectedRigidbody { get; set; }
    [field: SerializeField]
    private Renderer EnemyRenderer { get; set; }
    [field: SerializeField]
    private float NonRenderTimeToRespawn { get; set; }

    private float LastRendererTime { get; set; }
    private CustomTileManager TileManager { get; set; }
    private Transform Target { get; set; }
    private EnemyManager EnemyManager { get; set; }

    public void Initialize (Transform target, CustomTileManager tileManager, EnemyManager enemyManager)
    {
        Target = target;
        TileManager = tileManager;
        EnemyManager = enemyManager;
        LastRendererTime = Time.time;
    }

    protected virtual void Update ()
    {
        MoveEnemy();
        RespawnIfNotRendered();
    }

    protected virtual void MoveEnemy ()
    {
        ConnectedRigidbody.AddForce(((Target.position - transform.position).normalized * CharacterStats.MovementSpeed.CurrentValue.PresentValue) - ConnectedRigidbody.velocity, ForceMode.VelocityChange);
    }

    private void RespawnIfNotRendered ()
    {
        CheckRenderer();

        if (IsRenderedTooLongTimeAgo() == true)
        {
            LastRendererTime = Time.time;
            EnemyManager.RespawnEnemyInRandomPosition(this);
        }
    }

    private bool IsRenderedTooLongTimeAgo ()
    {
        return LastRendererTime + NonRenderTimeToRespawn < Time.time;
    }

    private void CheckRenderer ()
    {
        if (IsRendered() == true)
        {
            LastRendererTime = Time.time;
        }
    }

    private bool IsRendered ()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, EnemyRenderer.bounds) == true;
    }
}