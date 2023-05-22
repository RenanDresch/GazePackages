using System;

namespace Gaze.Utilities
{
    public interface IDestroyable
    {
        event Action OnDestroyEvent;
        void Destroy();
    }
}
