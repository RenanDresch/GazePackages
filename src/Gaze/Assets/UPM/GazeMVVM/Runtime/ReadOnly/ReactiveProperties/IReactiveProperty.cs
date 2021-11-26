using System;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveProperty<T>
    {
        T Value { get; }
        void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind = true);
        void UnbindAll();
    }
}
