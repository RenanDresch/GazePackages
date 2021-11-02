using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveList<T> : IReactiveList<T>
    {
        public readonly WriteableReactiveList<T> Write;
        IReactiveList<T> Read => Write;

        public ReactiveList(IEnumerable<T> content = default)
        {
            Write = new WriteableReactiveList<T>(content);
        }

        /// <summary>
        /// Get returns the IEnumerable
        /// Set overrides the internal List triggering OnChange
        /// </summary>
        public IEnumerable<T> Value
        {
            get => Read.Value;
            set => Write.Value = value;
        }
        
        public int Count => Read.Count;
        
        public void Add(T item) => Write.Add(item);
        public void Insert(int index, T item) => Write.Insert(index, item);
        public bool Remove(T item) => Write.Remove(item);
        public void RemoveAt(int index) => Write.RemoveAt(index);
        public void Clear() => Write.Clear();
        
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<T>> action,
            bool invokeOnBind = true) => Read.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action) =>
            Read.SafeBindOnAddAction(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action) =>
            Read.SafeBindOnRemoveAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) =>
            Read.SafeBindOnClearAction(destroyable, action);
    }
}
