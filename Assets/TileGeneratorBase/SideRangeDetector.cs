using System;
using UnityEngine;

namespace TileGenerator
{
    [Serializable]
    public class SideRangeDetector
    {
        [field: SerializeField]
        public WorldSide WorldSide { get; private set; }
        [field: SerializeField]
        public Transform DetectorTransform { get; private set; }
    }
}

