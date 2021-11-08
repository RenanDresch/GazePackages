using System;

namespace Gaze.Utilities
{
    public interface IDestroyable
    {
        public event Action OnDestroyEvent;
    }
}
