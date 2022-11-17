namespace Utils
{
    public class Resource<VariableType>
    {
        public ObservableVariable<VariableType> MaxValue { get; set; }
        public ObservableVariable<VariableType> CurrentValue { get; set; }

        public Resource (VariableType value)
        {
            MaxValue = new ObservableVariable<VariableType>(value);
            CurrentValue = new ObservableVariable<VariableType>(value);
        }

        public void ResetToInitialValue ()
        {
            CurrentValue.PresentValue = MaxValue.PresentValue;
        }
    }
}
