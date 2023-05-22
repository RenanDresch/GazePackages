using System;
using UnityEngine;

namespace Gaze.Utilities
{
    public class ObservableScriptableObject : ScriptableObject, IDestroyable, IEnableable
    {
        public event Action OnDestroyEvent;
        public event Action OnEnableEvent;
        public event Action OnDisableEvent;
        
        protected virtual void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }
        
        protected virtual void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
        
        protected virtual void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
            ReleaseEvents();
        }
        
        void ReleaseEvents()
        {
            OnEnableEvent = null;
            OnDisableEvent = null;
            OnDestroyEvent = null;
        }
        
        public void Destroy()
        {
            Destroy(this);
        }
    }
}
