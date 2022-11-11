using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveList<T> : IReactiveCollection<IReactiveList<T>, int, T, T, T>, IReleasable, IResettable
    {
        int IndexOf(T item);

        void Insert(int index, T item);

        void RemoveAt(int index);
        
        IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action);
        IReactiveList<T> UnbindOnInsertAction(Action<(int, IReactiveProperty<T>)> action);
        IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action);
        IReactiveList<T> UnbindOnRemoveAtAction(Action<(int, IReactiveProperty<T>)> action);
    }
}