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
        IReactiveStack<T> Reader => Writer;

        public IEnumerable<T> Value => Reader.Value;
        public int Count => Reader.Count;
        
        public ReactiveStack(T topItem = default) => Writer = new WriteableReactiveStack<T>(topItem);
        
        public T Peek() => Reader.Peek();
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<T>> action, bool invokeOnBind = true) =>
            Reader.SafeBindOnChangeAction(destroyable, action);
        public void SafeBindOnPushAction(IDestroyable destroyable, Action<T, T> action) =>
            Reader.SafeBindOnPushAction(destroyable, action);
        public void SafeBindOnPopAction(IDestroyable destroyable, Action<T, T> action) =>
            Reader.SafeBindOnPopAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) =>
            Reader.SafeBindOnClearAction(destroyable, action);
    }
}
