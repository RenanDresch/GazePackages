using Gaze.MVVM.Model.Read;
using Gaze.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.MVVM.Model
{
    [System.Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        [SerializeField]
        protected T currentValue;
        public virtual T Value
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }
        protected UnityEvent<T> OnPropertyChangeEvent { get; } = new UnityEvent<T>();

        public ReactiveProperty(T value = default)
        {
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
                Value = targetReactiveProperty.Value;
                targetReactiveProperty.SafeBindOnChangeAction(destroyable, SetValue);
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
        /// <param name="invokeOnBind">Should the action be invoked right after binding?</param>
        public void SafeBindOnChangeAction(IDestroyable destroyable, UnityAction<T> action, bool invokeOnBind = true)
        {
            if (destroyable != null)
            {
                OnPropertyChangeEvent.AddListener(action);
                if (invokeOnBind)
                {
                    OnPropertyChangeEvent.Invoke(Value);
                }

                destroyable.OnDestroyEvent.AddListener(() => OnPropertyChangeEvent.RemoveListener(action));
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }
        
        /// <summary>
        /// Unbinds the target ReactiveProperty OnChange trigger from this one.
        /// </summary>
        /// <param name="reactiveProperty">The ReactiveProperty that will unbind from this one</param>
        public void UnbindToReactiveProperty(ReactiveProperty<T> reactiveProperty)
        {
            reactiveProperty.UnbindOnChangeAction(SetValue);
        }
        
        /// <summary>
        /// Unbinds an OnChange Action from this Reactive Property. 
        /// </summary>
        /// <param name="action">The action to unbind</param>
        public void UnbindOnChangeAction(UnityAction<T> action)
        {
            OnPropertyChangeEvent.RemoveListener(action);
        }
        
        /// <summary>
        /// Unbind every ReactiveProperties and/or actions bound to this one.
        /// </summary>
        public void UnbindAll()
        {
            OnPropertyChangeEvent.RemoveAllListeners();
        }

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
            if (!forceUpdate && Equals(storedValue, value))
            {
                return;
            }
            currentValue = value;
            OnPropertyChangeEvent.Invoke(value);
        }

        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Value;
    }
}
