using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        [SerializeField]
        protected T currentValue;
        public T Value
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }

        protected readonly SafeAction<T> OnPropertyChangeEvent = new SafeAction<T>();

        protected readonly Func<T, T, bool> Comparer;
        
        public ReactiveProperty(T value = default, Func<T,T,bool> comparer = null)
        {
            Comparer = comparer ?? ((a, b) => Equals(a,b));
            currentValue = value;
        }
        
        /// <summary>
        /// Binds another reactive property to this, so when this property change triggers the target one triggers as well.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target ReactiveProperty.</param>
        /// <param name="targetReactiveProperty">The ReactiveProperty that will bind to this one.</param>
        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveProperty<T> targetReactiveProperty)
        {
            if (destroyable != null)
            {
                ForceUpdateValue(targetReactiveProperty.Value);
                targetReactiveProperty.SafeBindOnChangeAction(destroyable, ForceUpdateValue);
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }

        /// <summary>
        /// Binds an action to this Reactive Property so it's invoked whenever the OnChange triggers.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when this property changes.</param>
        /// <param name="invokeOnBind">True will invoke the action passing the current value stored into the ReactiveProperty right after binding it.</param>
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind = false)
        {
            if (OnPropertyChangeEvent.SafeBind(destroyable, action) && invokeOnBind)
            {
                action.Invoke(Value);
            }
        }

        /// <summary>
        /// Unbinds this ReactiveProperty OnChange trigger from the target one.
        /// </summary>
        /// <param name="reactivePropertyeProperty">The ReactiveProperty that will unbind from this one</param>
        /// <param name="reactiveProperty">The target reactive property to unbind from</param>
        /// <param name="resetOnUnbind">Should the value of this Reactive Property reset to default?</param>
        public void UnbindFromReactiveProperty(ReactiveProperty<T> reactiveProperty, bool resetOnUnbind = false)
        {
            reactiveProperty.UnbindOnChangeAction(ForceUpdateValue);
            if (resetOnUnbind)
            {
                ForceUpdateValue(default);
            }
        }

        /// <summary>
        /// Unbinds an OnChange Action from this Reactive Property. 
        /// </summary>
        /// <param name="action">The action to unbind</param>
        public void UnbindOnChangeAction(Action<T> action)
        {
            OnPropertyChangeEvent.Unbind(action);
        }

        /// <summary>
        /// Unbinds all OnChange Action from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public virtual void Release() => OnPropertyChangeEvent.UnbindAll();

        /// <summary>
        /// Passes a new value to this Reactive Property and forces it to invoke the OnChange trigger (even if the passed value is the same as the current one), invoking every listener as consequence.
        /// </summary>
        public void ForceUpdateValue(T newValue)
        {
            SetProperty(ref currentValue, newValue, true);
        }

        void SetProperty(ref T storedValue, T value, bool forceUpdate = false)
        {
            if (forceUpdate || !Comparer(storedValue, value))
            {
                currentValue = value;
                OnPropertyChangeEvent.Invoke(value);
            }
        }

        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;
    }
}
