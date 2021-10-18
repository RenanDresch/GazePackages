using UnityEngine.Events;

namespace Gaze.Utilities
{
    public interface IObservableMonoBehaviour
    {
        public UnityEvent OnAwakeEvent { get; }
        public UnityEvent OnStartEvent { get; }
        public UnityEvent OnEnableEvent { get; }
        public UnityEvent OnDisableEvent { get; }
        public UnityEvent OnDestroyEvent { get; }
    }
}
