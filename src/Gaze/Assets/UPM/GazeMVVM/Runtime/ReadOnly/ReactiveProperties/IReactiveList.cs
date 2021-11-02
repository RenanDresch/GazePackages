using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveList<T> : IReactiveProperty<IEnumerable<T>>
    {
        int Count { get; }
        void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
