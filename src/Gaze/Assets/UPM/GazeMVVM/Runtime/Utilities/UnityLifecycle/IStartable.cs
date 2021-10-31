using System;

namespace Gaze.Utilities
{
    public interface IStartable
    {
        public event Action OnStartEvent;
    }
}
