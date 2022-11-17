using UnityEngine;

public class Enemy : BaseCharacter
{
    [field: Space]
    [field: Header(nameof(Enemy))]
    [field: SerializeField]
    private Rigidbody ConnectedRigidbody { get; set; }
    [field: SerializeField]
    private Renderer EnemyRenderer { get; set; }
    [field: SerializeField]
    private float DesiredSpeed { get; set; }
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

    void Update ()
    {
        ConnectedRigidbody.AddForce(((Target.position - transform.position).normalized * DesiredSpeed) - ConnectedRigidbody.velocity, ForceMode.VelocityChange);
        RespawnIfNotRendered();
        //NavMeshAgent.SetDestination(Target.position);
    }

    private void RespawnIfNotRendered ()
    {
        CheckRenderer();

        if (LastRendererTime  + NonRenderTimeToRespawn < Time.time)
        {
            LastRendererTime = Time.time;
            EnemyManager.RespawnEnemyInRandomPosition(this);
            Debug.Log("Respawned");
        }
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
