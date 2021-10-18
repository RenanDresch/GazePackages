using System;
using Gaze.Utilities;

namespace Gaze.MVVM.Model.Read
{
    public interface IReactiveProperty<T>
    {
        public T Value { get; }
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind);
    }
}
