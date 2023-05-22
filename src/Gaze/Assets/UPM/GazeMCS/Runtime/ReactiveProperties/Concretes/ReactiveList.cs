using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveList<T> : ReactiveCollection<IReactiveList<T>, int, T, T, T>, IReactiveList<T>
    {
        readonly List<T> internalList;
        readonly List<T> initialCollection;
        
        readonly SafeAction<(int index, T newItem)> onInsert = new SafeAction<(int index, T newItem)>();
        readonly SafeAction<(int index, T item)> onRemoveAt = new SafeAction<(int index, T item)>();

        public ReactiveList(int capacity) => internalList = new List<T>(capacity);
        public ReactiveList(List<T> collection) => internalList = collection;

        protected override IReactiveList<T> Builder => this;
        
        public override bool TryGetValue(int index, out T value)
        {
            value = internalList[index];
            return value != null;
        }

        protected override T IndexGetter(int index) => internalList[index];
        
        protected override void IndexSetter(int index, T value) => internalList[index] = value;
        
        public override int Count => internalList.Count;

        protected override (int key, T value) AddToCollection(T item)
        {
            var key = internalList.Count;
            internalList.Add(item);
            OnCreateIndex(key, item);
            return (key, item);
        }

        protected override (bool, int key, T value) RemoveFromCollection(T item)
        {
            var itemIndex = IndexOf(item);
            var value = itemIndex != -1 ? internalList[itemIndex] : default;
            
            if (itemIndex != -1)
            {
                internalList.Remove(value);
            }

            return (itemIndex != -1, itemIndex, value);
        }

        public override bool Contains(T item) => IndexOf(item) != -1;

        protected override void ClearCollection() => internalList.Clear();

        public int IndexOf(T item)
        {
            for (var i = 0; i < internalList.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(internalList[i],item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            onInsert.Invoke((index, item));
        }

        public T RemoveAt(int index)
        {
            var removedItem = internalList[index];
            internalList.RemoveAt(index);
            onRemoveAt.Invoke((index, removedItem));
            return removedItem;
        }

        public IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, T)> action)
        {
            onInsert.SafeBind(destroyable, action);
            return Builder;
        }

        public IReactiveList<T> UnbindOnInsertAction(Action<(int, T)> action)
        {
            onInsert.Unbind(action);
            return this;
        }

        public IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int, T)> action)
        {
            onRemoveAt.SafeBind(destroyable, action);
            return Builder;
        }

        public IReactiveList<T> UnbindOnRemoveAtAction(Action<(int, T)> action)
        {
            onRemoveAt.Unbind(action);
            return this;
        }

        public override void Release()
        {
            base.Release();
            onInsert.Release();
            onRemoveAt.Release();
        }

        public void Reset()
        {
            internalList.Clear();
            if (initialCollection != null)
            {
                internalList.AddRange(initialCollection);
            }
        }
    }
}
