using UnityEngine;

namespace InfiniteTiles.Character
{
    public interface IDamageable
    {
        public abstract void GetDamaged (int damage);
        public abstract float CalculateRangeBetweenTarget (Vector3 attacker);
    }
}
