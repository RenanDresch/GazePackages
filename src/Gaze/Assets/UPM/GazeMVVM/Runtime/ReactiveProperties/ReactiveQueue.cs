using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveQueue<T> : ReactiveProperty<Queue<T>>, IReactiveQueue<T>
    {
        readonly SafeAction<T> onEnqueue = new SafeAction<T>();
        readonly SafeAction<T> onDequeue = new SafeAction<T>();
        readonly SafeAction onClear = new SafeAction();
        
        public ReactiveQueue(int capacity, T frontItem = default)
        {
            Value = new Queue<T>(capacity);
            if (frontItem != null)
            {
                Value.Enqueue(frontItem);
            }
        }

        public int Count => Value.Count;

        public T Peek() => Value.Peek();

        public void Queue(T item)
        {
            Value.Enqueue(item);
            OnPropertyChangeEvent.Invoke(Value);
            onEnqueue.Invoke(item);
        }
        
        public T Dequeue()
        {
            var item = Value.Dequeue();
            OnPropertyChangeEvent.Invoke(Value);
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
            Value.Clear();
            OnPropertyChangeEvent.Invoke(Value);
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
        
        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public void UnbindAllActions()
        {
            OnPropertyChangeEvent.UnbindAll();
            onEnqueue.UnbindAll();
            onDequeue.UnbindAll();
            onClear.UnbindAll();
        }
    }
}
