using System.Collections;
using System.Collections.Generic;
using TileGenerator;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CustomTileManager : TileManager<CustomTile>
{
    public delegate void NavMeshRebuildCallback (bool isStart);
    public event NavMeshRebuildCallback OnNavMeshRebuild;

    [field: SerializeField]
    private List<CustomTile> TilePrefabsCollection { get; set; } = new List<CustomTile>();
    [field: SerializeField]
    private TileConnector TileConectorPrefab { get; set; }
    private List<TileConnector> TileConnectors { get; set; } = new List<TileConnector>();
    [field: SerializeField]
    public NavMeshSurface NavMesh { get; private set; }

    protected override void Awake ()
    {
        AttachToEvents();
        base.Awake();
    }

    private void AttachToEvents ()
    {
        OnTileGenerated += HandleTileGenerated;
    }

    protected override CustomTile GenerateTile (Vector3 position, Vector2Int coords)
    {
        CustomTile generatedTile = Instantiate(GetRandomTile(), position, Quaternion.identity, transform);
        return generatedTile;
    }

    private void HandleTileGenerated (CustomTile generatedTile)
    {
        generatedTile.GameobjectsContainer.rotation = GetRandomRotation();
        Dictionary<WorldSide, CustomTile> sorroundingTiles = GetSorroundingTiles(generatedTile);

        foreach (KeyValuePair<WorldSide, CustomTile> singleTile in sorroundingTiles)
        {
            if (DoesConnectorAlreadyExists(generatedTile, singleTile.Value) == false)
            {
                TileConnector createdConnector = Instantiate(TileConectorPrefab);
                createdConnector.Initialize(generatedTile, singleTile.Value, singleTile.Key, GetReverseSide(singleTile.Key), SideSize);
                TileConnectors.Add(createdConnector);
            }
        }
    }

    private bool DoesConnectorAlreadyExists (CustomTile firstTile, CustomTile secondTile)
    {
        bool output = false;

        foreach (TileConnector singleConnector in TileConnectors)
        {
            output = singleConnector.IsThisConnectorConnectingTwoTiles(firstTile, secondTile);

            if (output == true)
            {
                break;
            }

        }

        return output;
    }

    private CustomTile GetRandomTile ()
    {
        return TilePrefabsCollection[Random.Range(0, TilePrefabsCollection.Count - 1)];
    }

    private Quaternion GetRandomRotation ()
    {
        return Quaternion.Euler(0, 90 * Random.Range(0, 3), 0);
    }

    private Dictionary<WorldSide, CustomTile> GetSorroundingTiles (CustomTile sourceTile)
    {
        Dictionary<WorldSide, CustomTile> output = new Dictionary<WorldSide, CustomTile>();

        AddToDictionaryBasingOnSide(sourceTile.Coords, WorldSide.NORTH, ref output);
        AddToDictionaryBasingOnSide(sourceTile.Coords, WorldSide.SOUTH, ref output);
        AddToDictionaryBasingOnSide(sourceTile.Coords, WorldSide.EAST, ref output);
        AddToDictionaryBasingOnSide(sourceTile.Coords, WorldSide.WEST, ref output);

        return output;
    }

    private void AddToDictionaryBasingOnSide (Vector2Int sourceCoords, WorldSide onWhatSideIsFoundTile, ref Dictionary<WorldSide, CustomTile> output)
    {
        Vector2Int tileOffset = sourceCoords + onWhatSideIsFoundTile.WorldSideToCoords();

        if (IsTileUnderIndex(tileOffset))
        {
            output[onWhatSideIsFoundTile] = (TilesDictionary[tileOffset]);
        }
    }

    private WorldSide GetReverseSide (WorldSide sourceSide)
    {
        WorldSide output = WorldSide.NONE;

        switch (sourceSide)
        {
            case WorldSide.NORTH:
                output = WorldSide.SOUTH;
                break;
            case WorldSide.SOUTH:
                output = WorldSide.NORTH;
                break;
            case WorldSide.WEST:
                output = WorldSide.EAST;
                break;
            case WorldSide.EAST:
                output = WorldSide.WEST;
                break;
            default:
                break;
        }

        return output;
    }
}
