using System;

namespace Gaze.Utilities
{
    public interface IAwakeable
    {
        public event Action OnAwakeEvent;
    }
}
