using System;
using System.Collections.Generic;

namespace DemonCastle.UI
{
    public class ObservableValue<T> : IReadOnlyObservableValue<T>
    {
        private T _value;
        public ObservableValue(T initialValue = default)
        {
            _value = initialValue;
        }

        public event Action<T> Changed;

        public T Value
        {
            get => _value;
            set 
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                Changed?.Invoke(value);
            }
        }
    }
}

