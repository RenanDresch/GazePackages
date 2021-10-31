using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        public T Value
        {
            get => Read.Value;
            set => write.Value = value;
        }
        readonly WriteableReactiveProperty<T> write;
        IReactiveProperty<T> Read => write;
        
        public ReactiveProperty(T initialValue = default)
        {
            write = new WriteableReactiveProperty<T>(initialValue);
        }
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind = true) => Read.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void ForceUpdateValue(T newValue) => write.ForceUpdateValue(newValue);
        public static implicit operator T(ReactiveProperty<T> reactiveProperty) => reactiveProperty.Read.Value;
    }
}
