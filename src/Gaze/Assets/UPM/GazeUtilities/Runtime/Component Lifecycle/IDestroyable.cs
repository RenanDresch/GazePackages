using UnityEngine.Events;

namespace Gaze.Utilities
{
    public interface IDestroyable
    {
        public UnityEvent OnDestroyEvent { get; }
    }
}
