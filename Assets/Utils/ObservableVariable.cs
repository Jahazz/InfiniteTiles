using System;

namespace Utils
{
    public class ObservableVariable<Type>
    {
        public delegate void VariableChangedArguments ();
        public event Action<Type> OnVariableChange = delegate { };

        private Type presentValue;
        public Type PresentValue {
            get { return presentValue; }
            set {
                bool hasValueChanged = HasValueChanged(value, presentValue);
                presentValue = value;

                if (hasValueChanged == true)
                {
                    OnVariableChange?.Invoke(presentValue);
                }
            }
        }

        public ObservableVariable (Type initialValue)
        {
            PresentValue = initialValue;
        }

        private bool HasValueChanged (Type oldValue, Type newValue)
        {

            return (oldValue == null && newValue != null) || (oldValue != null && newValue == null) || oldValue.Equals(newValue) == false;
        }
    }
}

