using UnityEngine;

namespace InfiniteTiles.Character
{
    public interface IDamageable
    {
        public abstract Transform GetTargetTransform (); 
        public abstract void GetDamaged (int damage);
    }
}
