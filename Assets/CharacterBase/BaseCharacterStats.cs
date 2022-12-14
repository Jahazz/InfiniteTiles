using Utils;

namespace InfiniteTiles.Character
{
    public class BaseCharacterStats<BaseCharacterDataType> where BaseCharacterDataType : BaseCharacterData
    {
        public Resource<int> Health { get; private set; }
        public Resource<float> BaseAttackSpeed { get; private set; }
        public ObservableVariable<string> CharacterName { get; private set; }
        public Resource<float> BaseCriticalChance { get; private set; }
        public Resource<int> Armor { get; private set; }
        public Resource<float> MovementSpeed { get; private set; }
        public Resource<float> BaseDamage { get; private set; }

        public virtual void InitializeBaseData (BaseCharacterDataType scriptableCharacterData)
        {
            Health = new Resource<int>(scriptableCharacterData.Health);
            BaseAttackSpeed = new Resource<float>(scriptableCharacterData.BaseAttackSpeed);
            BaseCriticalChance = new Resource<float>(scriptableCharacterData.BaseCriticalChance);
            Armor = new Resource<int>(scriptableCharacterData.Armor);
            CharacterName = new ObservableVariable<string>(scriptableCharacterData.Name);
            MovementSpeed = new Resource<float>(scriptableCharacterData.MovementSpeed);
            BaseDamage = new Resource<float>(scriptableCharacterData.BaseDamage);
        }
    }
}
