using Utils;

namespace InfiniteTiles.Character
{
    public class BaseCharacterStats<BaseCharacterDataType> where BaseCharacterDataType : BaseCharacterData
    {
        public Resource<int> Health { get; private set; }
        public Resource<int> AttacksPerSecond { get; private set; }
        public ObservableVariable<string> CharacterName { get; private set; }
        public Resource<float> CriticalChance { get; private set; }
        public Resource<int> Armor { get; private set; }
        public Resource<float> MovementSpeed { get; private set; }

        public virtual void InitializeBaseData (BaseCharacterDataType scriptableCharacterData)
        {
            Health = new Resource<int>(scriptableCharacterData.Health);
            AttacksPerSecond = new Resource<int>(scriptableCharacterData.AttacksPerSecond);
            CriticalChance = new Resource<float>(scriptableCharacterData.CriticalChance);
            Armor = new Resource<int>(scriptableCharacterData.Armor);
            CharacterName = new ObservableVariable<string>(scriptableCharacterData.Name);
            MovementSpeed = new Resource<float>(scriptableCharacterData.MovementSpeed);
        }
    }
}
