using UnityEngine;
using UnityEngine.Events;

namespace Gaze.Utilities
{
    public class ObservableMonoBehaviour : MonoBehaviour, IObservableMonoBehaviour
    {
        public UnityEvent OnAwakeEvent { get; } = new UnityEvent();
        public UnityEvent OnStartEvent { get; } = new UnityEvent();
        public UnityEvent OnEnableEvent { get; } = new UnityEvent();
        public UnityEvent OnDisableEvent { get; } = new UnityEvent();
        public UnityEvent OnDestroyEvent { get; } = new UnityEvent();
        
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
            
            OnAwakeEvent?.RemoveAllListeners();
            OnStartEvent?.RemoveAllListeners();
            OnEnableEvent?.RemoveAllListeners();
            OnDisableEvent?.RemoveAllListeners();
            OnDestroyEvent?.RemoveAllListeners();
        }
    }
}
