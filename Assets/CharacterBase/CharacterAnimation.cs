using System;
using UnityEngine;

namespace InfiniteTiles.Character.Animation
{
    [Serializable]
    public class CharacterAnimation
    {
        [field: SerializeField]
        public string AnimatorParameterName { get; private set; }
        [field: SerializeField]
        [field: Range(0.0f, 10.0f)]
        public float AnimationSpeedFactor { get; private set; }
        [field: SerializeField]
        public AnimationClip SetClip { get; set; }
    }
}

