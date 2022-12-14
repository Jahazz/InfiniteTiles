using System.Collections.Generic;
using TileGenerator;
using Unity.AI.Navigation;
using UnityEngine;

public class CustomTileManager : TileManager<CustomTile>
{
    [field: SerializeField]
    private List<CustomTile> TilePrefabsCollection { get; set; } = new List<CustomTile>();

    protected override void Awake ()
    {
        AttachToEvents();
        if (TilePrefabsCollection.Count > 0)
        {
            base.Awake();
        }
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
