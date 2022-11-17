using TileGenerator;
using UnityEngine;

public class CustomTile : Tile
{
    [field: SerializeField]
    public Transform GameobjectsContainer { get; private set; }
}
