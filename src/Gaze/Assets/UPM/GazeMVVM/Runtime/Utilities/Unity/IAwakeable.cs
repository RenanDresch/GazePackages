using System;

namespace Gaze.Utilities
{
    public interface IAwakeable
    {
        event Action OnAwakeEvent;
    }
}
