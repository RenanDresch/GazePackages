using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveQueue<T> : WriteableReactiveProperty<IEnumerable<T>>, IReactiveQueue<T>
    {
        Queue<T> internalQueue;

        SafeAction<T> onEnqueue = new SafeAction<T>();
        SafeAction<T> onDequeue = new SafeAction<T>();
        SafeAction onClear = new SafeAction();
        
        public WriteableReactiveQueue(T frontItem = default)
        {
            internalQueue = new Queue<T>();
            if (frontItem != null)
            {
                internalQueue.Enqueue(frontItem);
            }
        }

        public override IEnumerable<T> Value
        {
            get => internalQueue;
            set
            {
                internalQueue = new Queue<T>(value);
                OnPropertyChangeEvent.Invoke(internalQueue);
            }
        }

        public int Count => internalQueue.Count;

        public T Peek() => internalQueue.Peek();

        public void Queue(T item)
        {
            internalQueue.Enqueue(item);
            OnPropertyChangeEvent.Invoke(internalQueue);
            onEnqueue.Invoke(item);
        }
        
        public T Dequeue()
        {
            var item = internalQueue.Dequeue();
            OnPropertyChangeEvent.Invoke(internalQueue);
            if (Count < 1)
            {
                onClear.Invoke();
            }
            else
            {
                onDequeue.Invoke(item);
            }
            return item;
        }

        public void Clear()
        {
            internalQueue.Clear();
            OnPropertyChangeEvent.Invoke(internalQueue);
            onClear.Invoke();
        }

        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item is enqueued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets enqueued.</param>
        public void SafeBindOnEnqueueAction(IDestroyable destroyable, Action<T> action) => onEnqueue.SafeBind(destroyable, action);

        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item dequeued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.</param>
        public void SafeBindOnDequeueAction(IDestroyable destroyable, Action<T> action) => onDequeue.SafeBind(destroyable, action);

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => onClear.SafeBind(destroyable, action);
    }
}
