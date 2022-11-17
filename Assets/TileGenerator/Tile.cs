using UnityEngine;

namespace TileGenerator
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField]
        public MeshFilter AssignedMesh { get; private set; }
        public Vector2Int Coords { get; private set; }

        public virtual void Initialize (Vector2Int coords)
        {
            Coords = coords;
        }

        public float GetTileSize ()
        {
            return AssignedMesh.sharedMesh.bounds.size.x * transform.lossyScale.x;
        }
    }
}

