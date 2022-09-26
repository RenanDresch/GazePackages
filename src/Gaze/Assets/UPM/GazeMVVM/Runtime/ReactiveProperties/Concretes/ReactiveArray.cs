using System;
using System.Collections;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveArray<T> : IReactiveArray<T>
    {
        readonly IReactiveProperty<T>[] internalArray;

        readonly SafeAction<IReactiveArray<T>> onChange = new SafeAction<IReactiveArray<T>>();
        readonly SafeAction<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> onReplace = new SafeAction<(int,IReactiveProperty<T>,IReactiveProperty<T>)>();
        
        /// <summary>
        /// Count the internal Array (Array Lenght).
        /// </summary>
        public int Count => internalArray.Length;

        /// <summary>
        /// Instantiates a ReactiveArray with the required lenght.
        /// </summary>
        /// <param name="lenght">Array lenght.</param>
        public ReactiveArray(int lenght) => internalArray = new IReactiveProperty<T>[lenght];
        
        /// <summary>
        /// Instantiates a ReactiveArray passing an existing collection.
        /// </summary>
        /// <param name="collection">Collection to be used internally.</param>
        public ReactiveArray(IReactiveProperty<T>[] collection) => internalArray = collection;

        /// <summary>
        /// Gets the item at the internal Array index.
        /// Sets the item at the internal Array index, triggering OnChange and OnReplace, on that order.
        /// </summary>
        /// <param name="index">Index retrieve from.</param>
        public IReactiveProperty<T> this[int index]
        {
            get => internalArray[index];
            set
            {
                var replacedItem = internalArray[index];
                internalArray[index] = value;
                if (!Equals(replacedItem, value))
                {
                    onChange.Invoke(this);
                    onReplace.Invoke((index, value, replacedItem));
                }
            }
        }

        /// <summary>
        /// Binds an action to this Reactive Array so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the Array changes.</param>
        /// <returns>The ReactiveArray itself</returns>
        public IReactiveArray<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveArray<T>> action)
        {
            onChange.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Array so it's invoked whenever an item is replaced.
        /// The OnReplace callback passes the index of the replaced item, the new item stored into the Array and the replaced item that was formerly allocated at that array index.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the an Array element gets replaced.</param>
        /// <returns>The ReactiveArray itself</returns>
        public IReactiveArray<T> SafeBindOnReplaceItemAction(IDestroyable destroyable, Action<(int index, IReactiveProperty<T> newItem, IReactiveProperty<T> replacedItem)> action)
        {
            onReplace.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Unbinds all Actions from this Reactive Collection, allowing it to get collected. 
        /// </summary>
        public void Release()
        {
            onChange.Release();
            onReplace.Release();
        }
        
        public bool IsSynchronized => internalArray.IsSynchronized;
        public object SyncRoot => internalArray.SyncRoot;
        
        public void CopyTo(Array array, int index) => internalArray.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => internalArray.GetEnumerator();
    }
}
