using System.Collections;
using System.Collections.Generic;
using TileGenerator;
using UnityEngine;
using CodeBase;

public class EnemyManager : MonoBehaviour
{
    public delegate void EnemySpawnParameters(Enemy spawnedEnemy);
    public event EnemySpawnParameters OnEnemySpawned;

    [field: SerializeField]
    private List<Enemy> EnemiesToSpawn { get; set; } = new List<Enemy>();
    [field: SerializeField]
    private float TimeBetweenSpawns { get; set; }
    [field: SerializeField]
    private InfiniteTiles.Character.PlayerCharacter.CharacterController PlayerController { get; set; }
    [field: SerializeField]
    private CustomTileManager TileManager { get; set; }
    [field: SerializeField]
    private float SpawnRangeBehindBounds { get; set; }
    [field: SerializeField]
    private float SpawnRangeBehindBoundsOffset { get; set; }
    private List<Enemy> CurrentlyPresentEnemies { get; set; } = new List<Enemy>();

    private Rect NorthRect { get; set; }
    private Rect SouthRect { get; set; }
    private Rect EastRect { get; set; }
    private Rect WestRect { get; set; }
    private Rect extendedRangeRect { get; set; }

    public void Start ()
    {
        if (EnemiesToSpawn.Count > 0)
        {
            InitializeSpawnRects();
            StartCoroutine(EnemySpawnCoroutine());
        }
    }

    public void InitializeSpawnRects ()
    {
        float doubleSpawnRangeBehindBoundsOffset = (SpawnRangeBehindBoundsOffset * 2);
        Rect rangeRect = TileManager.RangeDetector.RangeDetectorSize;
        extendedRangeRect = new Rect(rangeRect.xMin - SpawnRangeBehindBoundsOffset, rangeRect.yMin - SpawnRangeBehindBoundsOffset, rangeRect.width + doubleSpawnRangeBehindBoundsOffset, rangeRect.height + doubleSpawnRangeBehindBoundsOffset);

        NorthRect = new Rect(extendedRangeRect.xMin, extendedRangeRect.yMax, extendedRangeRect.width + SpawnRangeBehindBounds, SpawnRangeBehindBounds);
        SouthRect = new Rect(extendedRangeRect.xMin - SpawnRangeBehindBounds, extendedRangeRect.yMin - SpawnRangeBehindBounds, extendedRangeRect.width + SpawnRangeBehindBounds, SpawnRangeBehindBounds);
        EastRect = new Rect(extendedRangeRect.xMax, extendedRangeRect.yMin - SpawnRangeBehindBounds, SpawnRangeBehindBounds, extendedRangeRect.height + SpawnRangeBehindBounds);
        WestRect = new Rect(extendedRangeRect.xMin - SpawnRangeBehindBounds, extendedRangeRect.yMin, SpawnRangeBehindBounds, extendedRangeRect.height + SpawnRangeBehindBounds);
    }

    public void RespawnEnemyInRandomPosition (Enemy enemy)
    {
        enemy.transform.position = GetRandomPointBehindBounds();
    }

    private IEnumerator EnemySpawnCoroutine ()
    {
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(TimeBetweenSpawns);
        }
    }

    private void SpawnRandomEnemy ()
    {
        Enemy enemyToSpawn = EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)];
        Enemy spawnedEnemy = Instantiate(enemyToSpawn, GetRandomPointBehindBounds(), Quaternion.identity);

        spawnedEnemy.Initialize(PlayerController, this);
        CurrentlyPresentEnemies.Add(spawnedEnemy);
        OnEnemySpawned?.Invoke(spawnedEnemy);
    }

    private Vector3 GetRandomPointBehindBounds ()
    {
        WorldSide worldSide = new List<WorldSide> { WorldSide.NORTH, WorldSide.SOUTH, WorldSide.EAST, WorldSide.WEST }.RandomElement();
        Rect currentRect = new Rect();

        switch (worldSide)
        {
            case WorldSide.NORTH:
                currentRect = NorthRect;
                break;
            case WorldSide.SOUTH:
                currentRect = SouthRect;
                break;
            case WorldSide.WEST:
                currentRect = WestRect;
                break;
            case WorldSide.EAST:
                currentRect = EastRect;
                break;
            default:
                break;
        }

        Vector3 rangeDetectorOffset = TileManager.RangeDetector.transform.position;
        Vector2 randomRectPosition = GetRandomPointOnRect(currentRect);

        return new Vector3(rangeDetectorOffset.x, 0, rangeDetectorOffset.z) + new Vector3(randomRectPosition.x, 0, randomRectPosition.y);
    }

    private void OnDrawGizmos ()
    {
        if (NorthRect != null)
        {
            //Gizmos.color = Color.yellow;
            //DrawRect(extendedRangeRect);
            //Gizmos.color = Color.blue;
            //DrawRect(TileManager.RangeDetector.RangeDetectorSize);
            Gizmos.color = Color.cyan;
            DrawRect(NorthRect);
            Gizmos.color = Color.yellow;
            DrawRect(SouthRect);
            Gizmos.color = Color.red;
            DrawRect(EastRect);
            Gizmos.color = Color.blue;
            DrawRect(WestRect);
        }
    }

    private void DrawRect (Rect rect)
    {
        Gizmos.DrawWireCube(new Vector3(rect.center.x, 0, rect.center.y), new Vector3(rect.width, 0, rect.height));
    }

    private Vector2 GetRandomPointOnRect (Rect rect)
    {
        return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
    }
}
