using UnityEngine.Events;

namespace Gaze.Utilities
{
    public interface IAwakeable
    {
        public UnityEvent OnAwakeEvent { get; }
    }
}
