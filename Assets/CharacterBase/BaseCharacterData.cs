using UnityEngine;

namespace InfiniteTiles.Character
{
    [CreateAssetMenu(fileName = "BaseCharacterData", menuName = "ScriptableObjects/ScriptableCharacterData", order = 1)]
    public class BaseCharacterData : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public int Health { get; private set; }
        [field: SerializeField]
        public int Energy { get; private set; }
        [field: SerializeField]
        public int Armor { get; private set; }
        [field: SerializeField]
        public int AttacksPerSecond { get; private set; }
        [field: SerializeField]
        public float CriticalChance { get; private set; }
        [field: SerializeField]
        public float MovementSpeed { get; private set; }
    }
}