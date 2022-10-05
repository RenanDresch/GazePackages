using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveProperty<T> : ICustomComparable<IReactiveProperty<T>, T>, IReleasable
    {
        T Value { get; set; }
        void ForceUpdateValue();
        IReactiveProperty<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action);
        IReactiveProperty<T> SafeBindOnChangeActionWithInvocation(IDestroyable destroyable, Action<T> action);
        IReactiveProperty<T> UnbindOnChangeAction(Action<T> action);
        IReactiveProperty<T> SafeBindOnReplaceAction(IDestroyable destroyable, Action<(T oldValue, T newValue)> action);
        IReactiveProperty<T> UnbindOnReplaceAction(Action<(T oldValue, T newValue)> action);
        bool IsEqualsTo(IReactiveProperty<T> other);
        bool CurrentValueIs(T value);
    }
}
