using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveStack<T> : IReadOnlyCollection<IReactiveProperty<T>>, IChangeable<IReactiveStack<T>>, IClearable<IReactiveStack<T>>, IPeekable<T>, IReleasable, IResettable
    {
        /// <summary>
        /// Pushes item to the stack top, triggering both OnChange and OnPush bindings, on that order
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The ReactiveStack itself</returns>
        IReactiveStack<T> Push(IReactiveProperty<T> item);
        /// <summary>
        /// Pops item from the stack top, triggering OnPop, OnChange and OnClear if stack becomes empty, on that order
        /// </summary>
        /// <returns>The popped item</returns>
        IReactiveProperty<T> Pop();
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is pushed into the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets pushed into the stack.</param>
        /// <returns>The ReactiveStack itself</returns>
        IReactiveStack<T> SafeBindOnPushAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is popped from the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.</param>
        /// <returns>The ReactiveStack itself</returns>
        IReactiveStack<T> SafeBindOnPopAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
    }
}
