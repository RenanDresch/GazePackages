using UnityEngine.Events;

namespace Gaze.Utilities
{
    public interface IStartable
    {
        public UnityEvent OnStartEvent { get; }
    }
}
