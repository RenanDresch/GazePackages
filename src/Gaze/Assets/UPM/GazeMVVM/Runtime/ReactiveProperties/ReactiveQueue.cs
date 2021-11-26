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

        public Queue<T> Value => new Queue<T>(Writer.Value);
        public int Count => Value.Count;
        
        public ReactiveQueue(T frontItem = default) => Writer = new WriteableReactiveQueue<T>(frontItem);
        
        public T Peek() => Value.Peek();
        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveQueue<T> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty.Writer);
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<Queue<T>> action, bool invokeOnBind = true) => 
            Writer.SafeBindOnChangeAction(destroyable, action);
        public void SafeBindOnEnqueueAction(IDestroyable destroyable, Action<T> action) => 
            Writer.SafeBindOnEnqueueAction(destroyable, action);
        public void SafeBindOnDequeueAction(IDestroyable destroyable, Action<T> action) => 
            Writer.SafeBindOnDequeueAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => 
            Writer.SafeBindOnClearAction(destroyable, action);
    }
}
