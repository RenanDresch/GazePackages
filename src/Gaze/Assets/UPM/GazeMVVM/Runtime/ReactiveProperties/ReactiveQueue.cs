using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveQueue<T> : IReactiveQueue<T>
    {
        public readonly WriteableReactiveQueue<T> Writer;
        IReactiveQueue<T> Reader => Writer;

        public ReactiveQueue(T frontItem = default)
        {
            Writer = new WriteableReactiveQueue<T>(frontItem);
        }
        
        public IEnumerable<T> Value => Reader.Value;

        public int Count => Reader.Count;
        public T Peek() => Reader.Peek();
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<T>> action, bool invokeOnBind = true) => Reader.SafeBindOnChangeAction(destroyable, action);
        public void SafeBindOnEnqueueAction(IDestroyable destroyable, Action<T> action) => Reader.SafeBindOnEnqueueAction(destroyable, action);
        public void SafeBindOnDequeueAction(IDestroyable destroyable, Action<T> action) => Reader.SafeBindOnDequeueAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => Reader.SafeBindOnClearAction(destroyable, action);
    }
}
