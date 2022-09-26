using System;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        [SerializeField]
        T formerValue;
        
        [SerializeField]
        protected T currentValue;
        
        public T Value
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }
        
        protected readonly SafeAction<T,T> OnPropertyChangeEvent = new SafeAction<T,T>();

        public ReactiveProperty(T value = default)
        {
            currentValue = value;
        }

        /// <summary>
        /// Binds an action to this Reactive Property so it's invoked whenever the OnChange triggers.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when this property changes.</param>
        public IReactiveProperty<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<T,T> action)
        {
            OnPropertyChangeEvent.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Invokes the passed action passing the stored values
        /// </summary>
        /// <param name="action">The action to be invoked.</param>
        public void Trigger(Action<T,T> action)
        {
            action(Value, formerValue);
        }
        
        /// <summary>
        /// Unbinds an OnChange Action from this Reactive Property. 
        /// </summary>
        /// <param name="action">The action to unbind</param>
        public void UnbindOnChangeAction(Action<T,T> action)
        {
            OnPropertyChangeEvent.Unbind(action);
        }

        /// <summary>
        /// Releases all OnChange Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public virtual void Release() => OnPropertyChangeEvent.Release();

        /// <summary>
        /// Passes a new value to this Reactive Property and forces it to invoke the OnChange trigger (even if the passed value is the same as the current one), invoking every listener as consequence.
        /// </summary>
        public void ForceUpdateValue(T newValue)
        {
            SetProperty(ref currentValue, newValue, true);
        }
        
        void SetValue(T newValue)
        {
            SetProperty(ref currentValue, newValue);
        }

        void SetProperty(ref T storedValue, T value, bool forceUpdate = false)
        {
            if (forceUpdate || !Equals(storedValue, value))
            {
                formerValue = storedValue;
                currentValue = value;
                OnPropertyChangeEvent.Invoke(value, formerValue);
            }
        }

        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;
    }
}
