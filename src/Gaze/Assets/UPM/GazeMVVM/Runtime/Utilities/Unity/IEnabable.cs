using System;

namespace Gaze.Utilities
{
    public interface IEnableable
    {
        event Action OnEnableEvent;
        event Action OnDisableEvent;
    }
}
