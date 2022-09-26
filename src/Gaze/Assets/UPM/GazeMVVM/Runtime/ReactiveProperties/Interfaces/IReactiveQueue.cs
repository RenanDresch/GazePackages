using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    public interface IReactiveQueue<T> : IReadOnlyCollection<IReactiveProperty<T>>, IPeekable<T>, IClearable, IReleasable
    {
        /// <summary>
        /// Enqueues the passed item
        /// </summary>
        /// <param name="item">Item to be queued</param>
        /// <returns>Returns the collection</returns>
        IReactiveQueue<T> Enqueue(IReactiveProperty<T> item);
        /// <summary>
        /// Dequeues the front item
        /// </summary>
        /// <returns>Returns the dequeued item</returns>
        IReactiveProperty<T> Dequeue();
        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the Queue changes.</param>
        IReactiveQueue<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveQueue<T>> action);
        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item is enqueued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets enqueued.</param>
        IReactiveQueue<T> SafeBindOnEnqueueAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
        /// <summary>
        /// Binds an action to this Reactive Queue so it's invoked whenever an item dequeued.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.</param>
        IReactiveQueue<T> SafeBindOnDequeueAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        IReactiveQueue<T> SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
