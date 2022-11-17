using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileGenerator
{
    public static class TileExtensions
    {
        public static Vector2Int WorldSideToCoords (this WorldSide worldSide)
        {
            Vector2Int output = Vector2Int.zero;

            switch (worldSide)
            {
                case WorldSide.NORTH:
                    output = new Vector2Int(0, 1);
                    break;
                case WorldSide.SOUTH:
                    output = new Vector2Int(0, -1);
                    break;
                case WorldSide.WEST:
                    output = new Vector2Int(-1, 0);
                    break;
                case WorldSide.EAST:
                    output = new Vector2Int(1, 0);
                    break;
                case WorldSide.NORTHWEST:
                    output = new Vector2Int(-1, 1);
                    break;
                case WorldSide.NORTHEAST:
                    output = new Vector2Int(1, 1);
                    break;
                case WorldSide.SOUTHWEST:
                    output = new Vector2Int(-1, -1);
                    break;
                case WorldSide.SOUTHEAST:
                    output = new Vector2Int(1, -1);
                    break;
                default:
                    break;
            }

            return output;
        }
    }
}

