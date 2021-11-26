using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveList<T> : IReactiveProperty<List<T>>
    {
        int Count { get; }
        T this[int index] { get; }
        void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action);
        void SafeBindOnReplaceAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action);
        void SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
