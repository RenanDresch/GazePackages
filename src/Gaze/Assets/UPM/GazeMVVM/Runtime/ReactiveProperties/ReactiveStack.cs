using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveStack<T> : IReactiveStack<T>
    {
        public readonly WriteableReactiveStack<T> Writer;

        public Stack<T> Value => new Stack<T>(Writer.Value);
        public int Count => Writer.Count;
        
        public ReactiveStack(T topItem = default) => Writer = new WriteableReactiveStack<T>(topItem);
        
        public T Peek() => Writer.Peek();
        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveStack<T> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty.Writer);
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<Stack<T>> action, bool invokeOnBind = true) =>
            Writer.SafeBindOnChangeAction(destroyable, action);
        public void SafeBindOnPushAction(IDestroyable destroyable, Action<T, T> action) =>
            Writer.SafeBindOnPushAction(destroyable, action);
        public void SafeBindOnPopAction(IDestroyable destroyable, Action<T, T> action) =>
            Writer.SafeBindOnPopAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) =>
            Writer.SafeBindOnClearAction(destroyable, action);
        
        public void UnbindFrom(ReactiveStack<T> targetReactiveProperty) =>
            Writer.UnbindFromReactiveProperty(targetReactiveProperty.Writer);
        public void Unbind() => Writer.UnbindAllActions();
    }
}
