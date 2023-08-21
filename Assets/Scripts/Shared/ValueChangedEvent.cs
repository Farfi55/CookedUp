using System;

namespace Shared
{
    public class ValueChangedEvent<T> : EventArgs {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public ValueChangedEvent(T oldValue, T newValue) {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
