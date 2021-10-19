using UnityEngine;
using UnityEngine.Events;

namespace Gaze.Utilities
{
    public class ObservableScriptableObject : ScriptableObject, IDestroyable, IEnableable
    {
        public UnityEvent OnDestroyEvent { get; } = new UnityEvent();
        public UnityEvent OnEnableEvent { get; } = new UnityEvent();
        public UnityEvent OnDisableEvent { get; } = new UnityEvent();
        protected virtual void OnEnable()
        {
            OnEnableEvent.Invoke();
        }
        protected virtual void OnDisable()
        {
            OnDisableEvent.Invoke();
        }
        protected virtual void OnDestroy()
        {
            OnDestroyEvent.Invoke();
        }
    }
}
