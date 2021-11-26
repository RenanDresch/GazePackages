using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveQueue<T> : IReactiveProperty<Queue<T>>
    {
        int Count { get; }
        T Peek();
        void SafeBindOnEnqueueAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnDequeueAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
