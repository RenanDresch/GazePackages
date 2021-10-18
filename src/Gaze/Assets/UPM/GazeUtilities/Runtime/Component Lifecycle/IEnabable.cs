using UnityEngine.Events;

namespace Gaze.Utilities
{
    public interface IEnableable
    {
        public UnityEvent OnEnableEvent { get; }
        public UnityEvent OnDisableEvent { get; }
    }
}
