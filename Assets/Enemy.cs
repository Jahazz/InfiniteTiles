using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCharacter
{
    [field: SerializeField]
    private NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField]
    private Rigidbody ConnectedRigidbody { get; set; }
    [field: SerializeField]
    private float DesiredSpeed { get; set; }
    [field: SerializeField]
    private Renderer EnemyRenderer { get; set; }
    private CustomTileManager TileManager { get; set; }
    private Transform Target { get; set; }
    private EnemyManager EnemyManager { get; set; }
    

    private bool IsTraversingOnMeshLink { get; set; }


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
    [field: SerializeField]
    private float NonRenderTimeToRespawn { get; set; }

    private float LastRendererTime { get; set; }

    private void CheckRenderer ()
    {
        if (IsRendered() == true)
        {
            LastRendererTime = Time.time;
        }
        else
        {

        }
    }

    private bool IsRendered ()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, EnemyRenderer.bounds) == true;
    }

    IEnumerator MoveAcrossNavMeshLink ()
    {
        OffMeshLinkData data = NavMeshAgent.currentOffMeshLinkData;
        NavMeshAgent.updateRotation = false;

        Vector3 startPos = NavMeshAgent.transform.position;
        Vector3 endPos = data.endPos;
        float duration = (endPos - startPos).magnitude / NavMeshAgent.velocity.magnitude;
        float t = 0.0f;
        float tStep = 1.0f / duration;
        while (t < 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            NavMeshAgent.destination = transform.position;
            t += tStep * Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
        NavMeshAgent.updateRotation = true;
        NavMeshAgent.CompleteOffMeshLink();
        IsTraversingOnMeshLink = false;

    }

    public void GetTileUnderCharacter ()
    {

    }



    private void StartLookingForTargets ()
    {
        StartCoroutine(LookForTarget());
    }

    private IEnumerator LookForTarget ()
    {
        while (true)
        {
            if (NavMeshAgent.isOnNavMesh == true)
            {
                NavMeshAgent.destination = Target.position;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
