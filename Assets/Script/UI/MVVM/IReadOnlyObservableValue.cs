using System;

namespace DemonCastle.UI
{
    public interface IReadOnlyObservableValue<T>
    {
        T Value { get; }
        event Action<T> Changed;
    }
}

