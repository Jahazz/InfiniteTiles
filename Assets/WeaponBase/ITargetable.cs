using InfiniteTiles.Character;

namespace InfiniteTiles.Weapon
{
    public interface ITargetable
    {
        public IDamageable CurrentTarget { get; set; }
    }
}

