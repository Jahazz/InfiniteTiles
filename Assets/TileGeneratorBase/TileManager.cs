using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileGenerator
{
    public abstract class TileManager<TileType> : MonoBehaviour where TileType : Tile
    {
        public delegate void TileEventArguments (TileType generatedTIle);
        public event TileEventArguments OnTileGenerated;

        [field: SerializeField]
        public TileRangeDetector RangeDetector { get; private set; }
        protected Dictionary<Vector2Int, TileType> TilesDictionary = new Dictionary<Vector2Int, TileType>();
        protected float SideSize { get; set; }

        protected abstract TileType GenerateTile (Vector3 position, Vector2Int coords);

        protected virtual void Awake ()
        {
            GenerateCenterTile();
        }

        protected virtual void Update ()
        {
            foreach (var item in RangeDetector.SideRangeDictionary)
            {
                CheckIfToGenerateTile(item.position);
            }
        }

        private void CheckIfToGenerateTile (Vector3 position)
        {
            Vector2Int checkedCoords = ConvertPositionToTileCoords(position, SideSize);

            if (IsTileUnderIndex(checkedCoords) == false)
            {
                GenerateTileAndAddToTiles(new Vector3(checkedCoords.x * SideSize, 0, checkedCoords.y * SideSize), checkedCoords);
            }
        }

        public TileType GetTileUnderPosition (Vector3 position)
        {
            return TilesDictionary[ConvertPositionToTileCoords(position, SideSize)];
        }

        protected bool IsTileUnderIndex (Vector2Int index)
        {
            return TilesDictionary.ContainsKey(index);
        }

        private void GenerateCenterTile ()
        {
            SideSize = GenerateTileAndAddToTiles(Vector3.zero, Vector2Int.zero).GetTileSize();
        }

        private TileType GenerateTileAndAddToTiles (Vector3 position, Vector2Int coords)
        {
            TileType generatedTile = GenerateTile(position, coords);
            generatedTile.Initialize(coords);
            TilesDictionary[coords] = generatedTile;
            OnTileGenerated?.Invoke(generatedTile);
            return generatedTile;
        }

        public static Vector2Int ConvertPositionToTileCoords (Vector3 position, float sideSize)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x / sideSize), Mathf.RoundToInt(position.z / sideSize));
        }
    }
}