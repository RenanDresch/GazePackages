using System;

namespace Gaze.Utilities
{
    public interface IEnableable
    {
        public event Action OnEnableEvent;
        public event Action OnDisableEvent;
    }
}
