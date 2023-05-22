using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveList<T> : IReactiveCollection<IReactiveList<T>, int, T, T, T>, IReleasable, IResettable
    {
        int IndexOf(T item);
        void Insert(int index, T item);
        T RemoveAt(int index);
        IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, T)> action);
        IReactiveList<T> UnbindOnInsertAction(Action<(int, T)> action);
        IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int, T)> action);
        IReactiveList<T> UnbindOnRemoveAtAction(Action<(int, T)> action);
    }
}
