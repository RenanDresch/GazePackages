using Gaze.Utilities;
using UnityEngine.Events;

namespace Gaze.MVVM.Model.Read
{
    public interface IReactiveProperty<T>
    {
        public T Value { get; }
        public void SafeBindOnChangeAction(IDestroyable destroyable, UnityAction<T> action, bool invokeOnBind);
    }
}
