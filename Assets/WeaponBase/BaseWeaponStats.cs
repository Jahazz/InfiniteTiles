using Utils;

namespace InfiniteTiles.Weapon
{
    public class BaseWeaponStats<BaseWeaponDataType> where BaseWeaponDataType : BaseWeaponData
    {
        public ObservableVariable<string> WeaponName { get; private set; }
        public ObservableVariable<float> AttackSpeed { get; private set; }
        public ObservableVariable<float> CriticalChance { get; private set; }
        public ObservableVariable<int> Damage { get; private set; }
        public ObservableVariable<float> Range { get; private set; }


        public virtual void InitializeBaseData (BaseWeaponDataType scriptableCharacterData)
        {
            WeaponName = new ObservableVariable<string>(scriptableCharacterData.WeaponName);
            AttackSpeed = new ObservableVariable<float>(scriptableCharacterData.AttackSpeed);
            CriticalChance = new ObservableVariable<float>(scriptableCharacterData.CriticalChance);
            Damage = new ObservableVariable<int>(scriptableCharacterData.Damage);
            Range = new ObservableVariable<float>(scriptableCharacterData.Range);
        }
    }
}
