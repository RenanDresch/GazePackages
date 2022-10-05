using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveList<T> : ReactiveCollection<IReactiveList<T>, int, T, T, T>, IReactiveList<T>
    {
        readonly List<IReactiveProperty<T>> internalList;
        
        readonly SafeAction<(int index, IReactiveProperty<T> newItem)> onInsert = new SafeAction<(int index, IReactiveProperty<T> newItem)>();
        readonly SafeAction<(int index, IReactiveProperty<T> item)> onRemoveAt = new SafeAction<(int index, IReactiveProperty<T> item)>();

        public ReactiveList(int capacity) => internalList = new List<IReactiveProperty<T>>(capacity);
        public ReactiveList(List<IReactiveProperty<T>> collection) => internalList = collection;

        protected override IReactiveList<T> Builder => this;
        
        public override bool TryGetValue(int index, out IReactiveProperty<T> value)
        {
            value = internalList[index];
            return value != null;
        }

        protected override IReactiveProperty<T> IndexGetter(int index) => internalList[index];
        
        protected override void IndexSetter(int index, IReactiveProperty<T> value) => internalList[index] = value;
        
        public override int Count => internalList.Count;

        protected override (int key, IReactiveProperty<T> value) AddToCollection(T item)
        {
            var key = internalList.Count;
            var value = new ReactiveProperty<T>(item);
            internalList.Add(value);
            OnCreateIndex(key, value);
            return (key, value);
        }

        protected override (bool, int key, IReactiveProperty<T> value) RemoveFromCollection(T item)
        {
            var itemIndex = IndexOf(item);
            var value = itemIndex != -1 ? internalList[itemIndex] : null;
            
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
                if (internalList[i].CurrentValueIs(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            var newReactiveItem = new ReactiveProperty<T>(item);
            internalList.Insert(index, newReactiveItem);
            onInsert.Invoke((index, newReactiveItem));
        }

        public void RemoveAt(int index)
        {
            var removedItem = internalList[index];
            internalList.RemoveAt(index);
            onRemoveAt.Invoke((index, removedItem));
        }

        public IReactiveList<T> SafeBindOnInsertAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action)
        {
            onInsert.SafeBind(destroyable, action);
            return Builder;
        }

        public IReactiveList<T> UnbindOnInsertAction(Action<(int, IReactiveProperty<T>)> action)
        {
            onInsert.Unbind(action);
            return this;
        }

        public IReactiveList<T> SafeBindOnRemoveAtAction(IDestroyable destroyable, Action<(int, IReactiveProperty<T>)> action)
        {
            onRemoveAt.SafeBind(destroyable, action);
            return Builder;
        }

        public IReactiveList<T> UnbindOnRemoveAtAction(Action<(int, IReactiveProperty<T>)> action)
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
    }
}
