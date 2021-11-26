using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveProperty<T> : IReactiveProperty<T>
    {
        [SerializeField]
        protected T currentValue;
        public T Value
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }
        
        protected readonly SafeAction<T> OnPropertyChangeEvent = new SafeAction<T>();

        public WriteableReactiveProperty(T value = default)
        {
            currentValue = value;
        }
        
        /// <summary>
        /// Binds another reactive property to this, so when this property change triggers the target one triggers as well.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target ReactiveProperty.</param>
        /// <param name="targetReactiveProperty">The ReactiveProperty that will bind to this one.</param>
        public virtual void SafeBindToReactiveProperty(IDestroyable destroyable, WriteableReactiveProperty<T> targetReactiveProperty)
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
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind = true)
        {
            if (OnPropertyChangeEvent.SafeBind(destroyable, action) && invokeOnBind)
            {
                action.Invoke(Value);
            }
        }

        /// <summary>
        /// Unbinds the target ReactiveProperty OnChange trigger from this one.
        /// </summary>
        /// <param name="writeableReactiveProperty">The ReactiveProperty that will unbind from this one</param>
        public void UnbindToReactiveProperty(WriteableReactiveProperty<T> writeableReactiveProperty)
        {
            writeableReactiveProperty.UnbindOnChangeAction(SetValue);
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
        public virtual void UnbindAllOnChangeActions() => OnPropertyChangeEvent.UnbindAll();

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
                currentValue = value;
                OnPropertyChangeEvent.Invoke(value);
            }
        }

        public static implicit operator T(WriteableReactiveProperty<T> writeableReactiveProperty) => writeableReactiveProperty.Value;
    }
}
