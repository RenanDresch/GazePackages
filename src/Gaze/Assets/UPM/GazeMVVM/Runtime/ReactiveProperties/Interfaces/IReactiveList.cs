using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveList<T> : IList<IReactiveProperty<T>>, IReleasable
    {
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the List changes.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveList<T>> action);
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets added.
        /// Not triggered by Insert.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is added.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnAddAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets inserted.
        /// Not triggered by Add.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is inserted.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action);
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets removed.
        /// Not triggered by RemoveAt.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is removed.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnRemoveAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action);
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets removed at.
        /// Not triggered by Remove.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is removed at.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action);
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever an item is replaced.
        /// The OnReplace callback passes the index of the replaced item, the new item stored into the List and the replaced item that got removed from that List index.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the an List element gets replaced.</param>
        /// <returns>The ReactiveList itself</returns>
        IReactiveList<T> SafeBindOnReplaceAction(IDestroyable destroyable, Action<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> action);
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        /// <returns>The ReactiveQueue itself</returns>
        IReactiveList<T> SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
