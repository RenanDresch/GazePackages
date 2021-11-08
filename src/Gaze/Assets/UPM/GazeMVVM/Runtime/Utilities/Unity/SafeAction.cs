using System;
using UnityEngine;

namespace Gaze.Utilities
{
    public class SafeAction
    {
        Action action;

        public bool SafeBind(IDestroyable destroyable, Action action)
        {
            var bindSuccessful = false;
            if (destroyable != null)
            {
                this.action += action;
                destroyable.OnDestroyEvent += () => this.action -= action;
                bindSuccessful = true;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
            return bindSuccessful;
        }

        public void Unbind(Action action)
        {
            this.action -= action;
        }
        
        public void Invoke()
        {
            action?.Invoke();
        }
    }
    
    public class SafeAction<T>
    {
        Action<T> action;

        public bool SafeBind(IDestroyable destroyable, Action<T> action)
        {
            var bindSuccessful = false;
            if (destroyable != null)
            {
                this.action += action;
                destroyable.OnDestroyEvent += () => this.action -= action;
                bindSuccessful = true;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
            return bindSuccessful;
        }

        public void Unbind(Action<T> action)
        {
            this.action -= action;
        }
        
        public void Invoke(T value)
        {
            action?.Invoke(value);
        }
    }
    
    public class SafeAction<T1,T2>
    {
        Action<T1,T2> action;

        public bool SafeBind(IDestroyable destroyable, Action<T1,T2> action)
        {
            var bindSuccessful = false;
            if (destroyable != null)
            {
                this.action += action;
                destroyable.OnDestroyEvent += () => this.action -= action;
                bindSuccessful = true;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
            return bindSuccessful;
        }

        public void Unbind(Action<T1,T2> action)
        {
            this.action -= action;
        }
        
        public void Invoke(T1 valueA, T2 valueB)
        {
            action?.Invoke(valueA, valueB);
        }
    }
}
