using System.Collections;
using System.Collections.Generic;
using TileGenerator;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CustomTile : Tile
{
    [field: SerializeField]
    public Transform GameobjectsContainer { get; private set; }
    [field: SerializeField]
    private Transform NorthOffMeshLink { get; set; }
    [field: SerializeField]
    private Transform SouthOffMeshLink { get; set; }
    [field: SerializeField]
    private Transform EastOffMeshLink { get; set; }
    [field: SerializeField]
    private Transform WesthOffMeshLink { get; set; }
    [field: SerializeField]
    public NavMeshData NavMeshData { get; private set; }

    public Dictionary<WorldSide, Transform> MeshLinksDictionary { get; private set; } = new Dictionary<WorldSide, Transform>();


    public override void Initialize (Vector2Int coords)
    {
        base.Initialize(coords);

        MeshLinksDictionary[WorldSide.NORTH] = NorthOffMeshLink;
        MeshLinksDictionary[WorldSide.SOUTH] = SouthOffMeshLink;
        MeshLinksDictionary[WorldSide.EAST] = EastOffMeshLink;
        MeshLinksDictionary[WorldSide.WEST] = WesthOffMeshLink;
    }
}
