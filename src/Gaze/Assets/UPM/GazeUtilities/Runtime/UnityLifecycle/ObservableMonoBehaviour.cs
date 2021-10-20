using System;
using UnityEngine;

namespace Gaze.Utilities
{
    public class ObservableMonoBehaviour : MonoBehaviour, IAwakeable, IStartable, IEnableable, IDestroyable
    {
        public event Action OnAwakeEvent;
        public event Action OnStartEvent;
        public event Action OnEnableEvent;
        public event Action OnDisableEvent;
        public event Action OnDestroyEvent;
        
        protected virtual void Awake()
        {
            OnAwakeEvent?.Invoke();
        }
        
        protected virtual void Start()
        {
            OnStartEvent?.Invoke();
        }
        
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
            OnAwakeEvent = null;
            OnStartEvent = null;
            OnEnableEvent = null;
            OnDisableEvent = null;
            OnDestroyEvent = null;
        }
    }
}
