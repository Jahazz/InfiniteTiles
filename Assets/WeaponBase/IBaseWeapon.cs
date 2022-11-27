using InfiniteTiles.Character;

namespace InfiniteTiles.Weapon
{
    public interface IBaseWeapon
    {
        public delegate void HitEventParameters (IDamageable target);
        public event HitEventParameters OnAttackStart;

        public void DealDamage (IDamageable target);
        public void Initialize (IBaseCharacter owner);
        public IDamageable CurrentTarget { get; set; }
    }
}

