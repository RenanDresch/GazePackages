using System;
using System.Collections;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveArray<T>
            : IReactiveIndexable<IReactiveArray<T>, int, T>,
              ICollection,
              IReleasable,
              IResettable
    {
        IReactiveArray<T> SafeBindOnSetAction(IDestroyable destroyable, Action<int, IReactiveProperty<T>> action);
        IReactiveArray<T> UnbindOnSetAction(Action<int, IReactiveProperty<T>> action);
        IReactiveArray<T> SafeBindOnReplaceAction(IDestroyable destroyable, Action<int, IReactiveProperty<T>, IReactiveProperty<T>> action);
        IReactiveArray<T> UnbindOnReplaceAction(Action<int, IReactiveProperty<T>, IReactiveProperty<T>> action);
    }
}
