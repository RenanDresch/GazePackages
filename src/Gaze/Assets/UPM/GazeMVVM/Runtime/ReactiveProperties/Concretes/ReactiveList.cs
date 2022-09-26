using System;
using System.Collections;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveList<T> : IReactiveList<T>
    {
        readonly List<IReactiveProperty<T>> internalList;

        readonly SafeAction<IReactiveList<T>> onChange = new SafeAction<IReactiveList<T>>();
        readonly SafeAction<IReactiveProperty<T>> onAdd = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction<(int index, IReactiveProperty<T> newItem)> onInsert = new SafeAction<(int index, IReactiveProperty<T> newItem)>();
        readonly SafeAction<IReactiveProperty<T>> onRemove = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction<(int index, IReactiveProperty<T> item)> onRemoveAt = new SafeAction<(int index, IReactiveProperty<T> item)>();
        readonly SafeAction<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> onReplace = new SafeAction<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)>();
        readonly SafeAction onClear = new SafeAction();

        /// <summary>
        /// Instantiates a ReactiveList with the required capacity.
        /// </summary>
        /// <param name="capacity">Internal List capacity.</param>
        public ReactiveList(int capacity) => internalList = new List<IReactiveProperty<T>>(capacity);
        
        /// <summary>
        /// Instantiates a ReactiveList passing an existing collection.
        /// </summary>
        /// <param name="collection">Collection to be used internally.</param>
        public ReactiveList(List<IReactiveProperty<T>> collection) => internalList = collection;
        
        /// <summary>
        /// Gets the item at the internal List index.
        /// Sets the item at the internal List index, triggering OnChange and OnReplace, on that order.
        /// </summary>
        /// <param name="index">Index retrieve from.</param>
        public IReactiveProperty<T> this[int index]
        {
            get => internalList[index];
            set
            {
                var oldValue = internalList[index];
                internalList[index] = value;
                if (!Equals(oldValue, value))
                {
                    onChange.Invoke(this);
                    onReplace.Invoke((index, value, oldValue));
                }
            }
        }
        
        /// <summary>
        /// Count the internal List.
        /// </summary>
        public int Count => internalList.Count;
        
        /// <summary>
        /// Gets the internal index of a item.
        /// </summary>
        /// <param name="item">Item to search for</param>
        /// <returns>Index of the internal List or -1 if it does not exist.</returns>
        public int IndexOf(IReactiveProperty<T> item) => internalList.IndexOf(item);
        
        /// <summary>
        /// Validates if an Item exist withing te internal List.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>true if it exist false if dont.</returns>
        public bool Contains(IReactiveProperty<T> item) => internalList.Contains(item);

        /// <summary>
        /// Adds an item to the internal List, triggering OnChange and OnAdd, on that order.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(IReactiveProperty<T> item)
        {
            internalList.Add(item);
            onChange.Invoke(this);
            onAdd.Invoke(item);
        }
        
        /// <summary>
        /// Clears the internal List, triggering OnChange and OnClear, on that order.
        /// </summary>
        public void Clear()
        {
            internalList.Clear();
            onChange.Invoke(this);
            onClear.Invoke();
        }
        
        
        /// <summary>
        /// Removes an item from the internal List, triggering OnChange, OnRemove, and OnClear if the internal List becomes empty, on that order.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public bool Remove(IReactiveProperty<T> item)
        {
            var result = internalList.Remove(item);
            if (result)
            {
                onChange.Invoke(this);
                onRemove.Invoke(item);
                if (Count < 1)
                {
                    onClear.Invoke();
                }
            }
            return result;
        }
        
        /// <summary>
        /// Inserts and item into the internal List, triggering OnChange and OnInsert, on that order.
        /// </summary>
        /// <param name="index">Index to insert at.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, IReactiveProperty<T> item)
        {
            internalList.Insert(index, item);
            onChange.Invoke(this);
            onInsert.Invoke((index, item));
        }
        
        /// <summary>
        /// Remove an item from the internal List, triggering OnChange, OnRemoveAt and OnClear if the internal List becomes empty, on that order.
        /// </summary>
        /// <param name="index">Index to remove from.</param>
        public void RemoveAt(int index)
        {
            var removedItem = internalList[index];
            internalList.RemoveAt(index);
            onChange.Invoke(this);
            onRemoveAt.Invoke((index,removedItem));
            if (Count < 1)
            {
                onClear.Invoke();
            }
        }

        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the List changes.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveList<T>> action)
        {
            onChange.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets added.
        /// Not triggered by Insert.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is added.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnAddAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onAdd.SafeBind(destroyable, action);
            return this;
        }
        
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets inserted.
        /// Not triggered by Add.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is inserted.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action)
        {
            onInsert.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets removed.
        /// Not triggered by RemoveAt.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is removed.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnRemoveAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onRemove.SafeBind(destroyable, action);
            return this;
        }
        
        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever and item gets removed at.
        /// Not triggered by Remove.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item is removed at.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int,IReactiveProperty<T>)> action)
        {
            onRemoveAt.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive List so it's invoked whenever an item is replaced.
        /// The OnReplace callback passes the index of the replaced item, the new item stored into the List and the replaced item that got removed from that List index.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the an List element gets replaced.</param>
        /// <returns>The ReactiveList itself</returns>
        public IReactiveList<T> SafeBindOnReplaceAction(IDestroyable destroyable,
            Action<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> action)
        {
            onReplace.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        /// <returns>The ReactiveQueue itself</returns>
        public IReactiveList<T> SafeBindOnClearAction(IDestroyable destroyable, Action action)
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
            onAdd.Release();
            onInsert.Release();
            onRemove.Release();
            onRemoveAt.Release();
            onReplace.Release();
            onClear.Release();
        }
        
        public void CopyTo(IReactiveProperty<T>[] array, int arrayIndex) => internalList.CopyTo(array, arrayIndex);
        public bool IsReadOnly => false;
        public IEnumerator<IReactiveProperty<T>> GetEnumerator() => internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => internalList.GetEnumerator();
    }
}
