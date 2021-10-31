using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveStack<T> : IReactiveStack<T>
    {
        public readonly WriteableReactiveStack<T> Write;
        IReactiveStack<T> Read => Write;

        public ReactiveStack(T topItem = default)
        {
            Write = new WriteableReactiveStack<T>(topItem);
        }

        /// <summary>
        /// Get returns a new instance of the Stack, not the internal one.
        /// Set overrides the internal Stack triggering OnChange
        /// </summary>
        public Stack<T> Value
        {
            get => new Stack<T>(Read.Value);
            set => Write.Value = value;
        }
        public int Count => Read.Count;
        public T Peek() => Read.Peek();
        public void Push(T item) => Write.Push(item);
        public T Pop() => Write.Pop();
        public void Clear() => Write.Clear();
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<Stack<T>> action, bool invokeOnBind = true) => Read.SafeBindOnChangeAction(destroyable, action);
        public void SafeBindOnPushAction(IDestroyable destroyable, Action<T, T> action) => Read.SafeBindOnPushAction(destroyable, action);
        public void SafeBindOnPopAction(IDestroyable destroyable, Action<T, T> action) => Read.SafeBindOnPopAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => Read.SafeBindOnClearAction(destroyable, action);
    }
}
