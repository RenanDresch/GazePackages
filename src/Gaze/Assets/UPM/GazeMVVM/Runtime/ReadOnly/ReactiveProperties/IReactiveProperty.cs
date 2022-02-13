using System;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveProperty<T>
    {
        void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind);
        void UnbindOnChangeAction(Action<T> action);
        void Release();
    }
}
