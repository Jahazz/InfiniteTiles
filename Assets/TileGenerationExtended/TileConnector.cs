using System.Collections;
using System.Collections.Generic;
using TileGenerator;
using Unity.AI.Navigation;
using UnityEngine;

public class TileConnector : MonoBehaviour
{
    [field: SerializeField]
    private NavMeshLink NavMeshLink { get; set; }


    private CustomTile FirstTile { get; set; }
    private CustomTile SecondTile { get; set; }

    public void Initialize (CustomTile firstTile, CustomTile secondTile, WorldSide firstTileSide, WorldSide secondTileSide, float tileSize)
    {
        FirstTile = firstTile;
        SecondTile = secondTile;
        NavMeshLink.width = tileSize;
        NavMeshLink.startPoint = FirstTile.MeshLinksDictionary[firstTileSide].position;
        NavMeshLink.endPoint = SecondTile.MeshLinksDictionary[secondTileSide].position;
    }

    public bool IsThisConnectorConnectingTwoTiles (CustomTile firstTile, CustomTile secondTIle)
    {
        return (FirstTile == firstTile && SecondTile == secondTIle) || (FirstTile == secondTIle && SecondTile == firstTile);
    }

    public CustomTile GetOtherTile (CustomTile tile)
    {
        return tile == FirstTile ? FirstTile : SecondTile;
    }
}
