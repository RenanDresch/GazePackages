using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveStack<T> : IReactiveProperty<Stack<T>>
    {
        int Count { get; }
        T Peek();
        void SafeBindOnPushAction(IDestroyable destroyable, Action<T, T> action);
        void SafeBindOnPopAction(IDestroyable destroyable, Action<T, T> action);
        void SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
