using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        public T Value => Reader.Value;
        public readonly WriteableReactiveProperty<T> Writer;
        IReactiveProperty<T> Reader => Writer;
        
        public ReactiveProperty(T initialValue = default) => Writer = new WriteableReactiveProperty<T>(initialValue);
        
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T> action, bool invokeOnBind = true) =>
            Reader.SafeBindOnChangeAction(destroyable, action, invokeOnBind);

        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveProperty<T> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty.Writer);

        public void UnbindFrom(ReactiveProperty<T> targetReactiveProperty) =>
            Writer.UnbindFromReactiveProperty(targetReactiveProperty.Writer);
        public void UnbindAll() => Writer.UnbindAll();
        
        public static implicit operator T(ReactiveProperty<T> reactiveProperty) =>
            reactiveProperty.Reader.Value;
    }
}
