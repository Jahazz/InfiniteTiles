using InfiniteTiles.Character;
using InfiniteTiles.Weapon;
using UnityEngine;
using static InfiniteTiles.Weapon.IBaseWeapon;

public class Enemy : BaseCharacter<BaseCharacterStats<BaseCharacterData>, BaseCharacterData>
{
    public event HitEventParameters OnAttackStart;

    [field: Space]
    [field: Header(nameof(Enemy))]
    [field: SerializeField]
    private Renderer EnemyRenderer { get; set; }
    [field: SerializeField]
    private float NonRenderTimeToRespawn { get; set; }

    private float LastRendererTime { get; set; }
    private EnemyManager EnemyManager { get; set; }
    public IDamageable CurrentTarget { get; set; }

    public void Initialize (IDamageable target, EnemyManager enemyManager)
    {
        CurrentTarget = target;
        EnemyManager = enemyManager;
        LastRendererTime = Time.time;

        Initialize();

        CurrentCharacterSpeed = CharacterStats.MovementSpeed.CurrentValue.PresentValue;
    }

    protected override void Update ()
    {
        RespawnIfNotRendered();
        UpdateModelRotation();

        base.Update();
    }

    protected override void InitializeWeapons ()
    {
        base.InitializeWeapons();

        foreach (IBaseWeapon weapon in WeaponsCollection)
        {
            weapon.CurrentTarget = CurrentTarget;
        }
    }

    private void UpdateModelRotation ()
    {
        Vector3 targetPostition = CurrentTarget.GetTargetTransform().position;
        targetPostition.y = transform.position.y;
        transform.LookAt(targetPostition);
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