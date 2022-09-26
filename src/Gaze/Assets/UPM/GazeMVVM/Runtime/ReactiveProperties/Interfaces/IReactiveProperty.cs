using System;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    public interface IReactiveProperty<out T> : ITriggerable<T,T>, IReleasable
    {
        T Value { get; }
        IReactiveProperty<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<T,T> action);
    }
}
