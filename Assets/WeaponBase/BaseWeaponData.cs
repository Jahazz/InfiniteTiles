using UnityEngine;

namespace InfiniteTiles.Weapon
{
    [CreateAssetMenu(fileName = "BaseWeaponData", menuName = "ScriptableObjects/BaseWeaponData", order = 1)]
    public class BaseWeaponData : ScriptableObject
    {
        [field: SerializeField]
        public string WeaponName { get; private set; }
        [field: SerializeField]
        public float AttackSpeed { get; private set; }
        [field: SerializeField]
        public float CriticalChance { get; private set; }
        [field: SerializeField]
        public int Damage { get; private set; }
        [field: SerializeField]
        public float Range { get; private set; }
    }
}