using System;
using System.Collections;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    public interface IReactiveArray<T> : ICollection, IReleasable
    {
        /// <summary>
        /// Gets the item at the internal Array index
        /// Sets the item at the internal Array index, triggering OnChange and OnReplace, on that order
        /// </summary>
        /// <param name="index"></param>
        IReactiveProperty<T> this[int index] { get; set; }
        /// <summary>
        /// Binds an action to this Reactive Array so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the Array changes.</param>
        /// <returns>The ReactiveArray itself</returns>
        IReactiveArray<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveArray<T>> action);
        /// <summary>
        /// Binds an action to this Reactive Array so it's invoked whenever an item is replaced.
        /// The OnReplace callback passes the index of the replaced item, the new item stored into the Array and the replaced item that got removed from that Array index.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the an Array element gets replaced.</param>
        /// <returns>The ReactiveArray itself</returns>
        IReactiveArray<T> SafeBindOnReplaceItemAction(IDestroyable destroyable, Action<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> action);
    }
}
