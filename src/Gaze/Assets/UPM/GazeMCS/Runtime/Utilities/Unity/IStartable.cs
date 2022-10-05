using System;

namespace Gaze.Utilities
{
    public interface IStartable
    {
        event Action OnStartEvent;
    }
}
