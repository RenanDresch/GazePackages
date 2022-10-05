using System;
using System.Collections;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveQueue<T> : IReactiveQueue<T>
    {
        readonly Queue<IReactiveProperty<T>> internalQueue;

        readonly SafeAction<IReactiveQueue<T>> onChange = new SafeAction<IReactiveQueue<T>>();
        readonly SafeAction<IReactiveProperty<T>> onEnqueue = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction<IReactiveProperty<T>> onDequeue = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction onClear = new SafeAction();

        /// <summary>
        /// Count the internal Queue 
        /// </summary>
        public int Count => internalQueue.Count;
        
        /// <summary>
        /// Peeks the internal Queue
        /// </summary>
        public IReactiveProperty<T> Peek => internalQueue.Peek();

        /// <summary>
        /// Instantiates a ReactiveQueue with the required capacity
        /// </summary>
        /// <param name="capacity">Queue capacity</param>
        public ReactiveQueue(int capacity) => internalQueue = new Queue<IReactiveProperty<T>>(capacity);

        /// <summary>
        /// Instantiates a ReactiveQueue passing an existing collection
        /// </summary>
        /// <param name="queue">Collection to be used internally</param>
        public ReactiveQueue(Queue<IReactiveProperty<T>> queue) => internalQueue = queue;

        /// <summary>
        /// Enqueues the passed item, triggering OnChange and OnEnqueue, on that order
        /// </summary>
        /// <param name="item">Item to be queued</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveQueue<T> Enqueue(IReactiveProperty<T> item)
        {
            internalQueue.Enqueue(item);
            onChange.Invoke(this);
            onEnqueue.Invoke(item);
            return this;
        }

        /// <summary>
        /// Dequeues the front item, triggering OnChange, OnDequeue and OnClear if Queue becomes empty, on that order
        /// </summary>
        /// <returns>Returns the dequeued item</returns>
        public IReactiveProperty<T> Dequeue()
        {
            var item = internalQueue.Dequeue();
            onChange.Invoke(this);
            onDequeue.Invoke(item);
            if (Count < 1)
            {
                onClear.Invoke();
            }
            return item;
        }
        
        /// <summary>
        /// Clears the Queue
        /// </summary>
        public void Clear()
        {
            internalQueue.Clear();
            onChange.Invoke(this);
            onClear.Invoke();
        }
        
        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the Queue changes.</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveQueue<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveQueue<T>> action)
        {
            onChange.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveQueue<T> UnbindOnChangeAction(Action<IReactiveQueue<T>> action)
        {
            onChange.Unbind(action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item is enqueued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets enqueued.</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveQueue<T> SafeBindOnEnqueueAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onEnqueue.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item dequeued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveQueue<T> SafeBindOnDequeueAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onDequeue.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveQueue<T> SafeBindOnClearAction(IDestroyable destroyable, Action action)
        {
            onClear.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Unbinds all Actions from this Reactive Collection, allowing it to get collected. 
        /// </summary>
        public void Release()
        {
            onChange.Release();
            onEnqueue.Release();
            onDequeue.Release();
            onClear.Release();
        }
        
        public IEnumerator<IReactiveProperty<T>> GetEnumerator() => internalQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => internalQueue.GetEnumerator();
    }
}
