using System;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveProperty<T> : ReactiveComparable<IReactiveProperty<T>, T>, IReactiveProperty<T>
    {
        readonly Guid propertyId = Guid.NewGuid();
        readonly T initialValue;
        
        [SerializeField]
        T currentValue;

        protected override IReactiveProperty<T> Builder => this;
        
        public T Value
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }
        
        protected readonly SafeAction<T> OnPropertyChangeEvent = new SafeAction<T>();
        protected readonly SafeAction<(T oldValue, T newItem)> OnPropertyReplaceEvent = new SafeAction<(T oldValue, T newItem)>();

        public ReactiveProperty(T value = default)
        {
            initialValue = currentValue = value;
        }

        public void ForceUpdateValue()
        {
            OnPropertyChangeEvent.Invoke(currentValue);
        }
        
        public void ForceUpdateWithValue(T value)
        { 
            SetProperty(ref currentValue, value, true);
        }

        public IReactiveProperty<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action)
        {
            OnPropertyChangeEvent.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveProperty<T> SafeBindOnChangeActionWithInvocation(IDestroyable destroyable, Action<T> action)
        {
            action.Invoke(currentValue);
            return SafeBindOnChangeAction(destroyable, action);
        }

        IReactiveProperty<T> IReactiveProperty<T>.UnbindOnChangeAction(Action<T> action)
        {
            OnPropertyChangeEvent.Unbind(action);
            return this;
        }

        public IReactiveProperty<T> SafeBindOnReplaceAction(IDestroyable destroyable, Action<(T oldValue, T newValue)> action)
        {
            OnPropertyReplaceEvent.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveProperty<T> UnbindOnReplaceAction(Action<(T oldValue, T newValue)> action)
        {
            OnPropertyReplaceEvent.Unbind(action);
            return this;
        }

        public bool IsEqualsTo(IReactiveProperty<T> other) => IsEquals(currentValue, other.Value);

        public bool CurrentValueIs(T value) => IsEquals(currentValue, value);

        public void UnbindOnChangeAction(Action<T> action) => OnPropertyChangeEvent.Unbind(action);

        public override void Release() => OnPropertyChangeEvent.Release();
        
        public void ForceUpdateValue(T newValue) => SetProperty(ref currentValue, newValue, true);

        void SetValue(T newValue) => SetProperty(ref currentValue, newValue);

        void SetProperty(ref T storedValue, T value, bool forceUpdate = false)
        {
            if (forceUpdate || !IsEquals(storedValue, value))
            {
                var formerValue = storedValue;
                currentValue = value;
                OnPropertyChangeEvent.Invoke(value);
                OnPropertyReplaceEvent.Invoke((formerValue, value));
            }
        }

        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;

        public override bool Equals(object obj)
        {
            var otherProperty = (ReactiveProperty<T>)obj;
            return otherProperty?.propertyId == propertyId;
        }
        
        protected bool Equals(ReactiveProperty<T> other) =>  other.propertyId == propertyId;

        public override int GetHashCode()
        {
            var hashCode = propertyId.GetHashCode();
            return hashCode;
        }

        public void Reset()
        {
            Value = initialValue;
        }
    }
}
