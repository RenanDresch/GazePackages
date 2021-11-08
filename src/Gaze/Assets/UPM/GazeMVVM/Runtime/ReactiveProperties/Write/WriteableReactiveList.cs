using System;
using System.Collections;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveList<T> : WriteableReactiveProperty<IEnumerable<T>>, IReactiveList<T>, IList<T>
    {
        List<T> internalList;

        SafeAction<T> onAdd = new SafeAction<T>();
        SafeAction<T> onRemove = new SafeAction<T>();
        SafeAction onClear = new SafeAction();
        
        public WriteableReactiveList(IEnumerable<T> content = null)
        {
            currentValue = internalList = new List<T>();
            if (content != null)
            {
                foreach (var t in content)
                {
                    internalList.Add(t);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => internalList.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => internalList.IndexOf(item);
        public bool Contains(T item) => internalList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => internalList.CopyTo(array, arrayIndex);
        
        public void Add(T item)
        {
            internalList.Add(item);
            OnPropertyChangeEvent.Invoke(internalList);
            onAdd.Invoke(item);
        }
        public void Clear()
        {
            internalList.Clear();
            OnPropertyChangeEvent.Invoke(internalList);
            onClear.Invoke();
        }
        public bool Remove(T item)
        {
            var result = internalList.Remove(item);
            if (result)
            {
                OnPropertyChangeEvent.Invoke(internalList);
                onRemove.Invoke(item);
            }
            return result;
        }
        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            OnPropertyChangeEvent.Invoke(internalList);
            onAdd.Invoke(item);
        }
        public void RemoveAt(int index)
        {
            var removedItem = internalList[index];
            internalList.RemoveAt(index);
            OnPropertyChangeEvent.Invoke(internalList);
            onRemove.Invoke(removedItem);
        }

        public T this[int index]
        {
            get => internalList[index];
            set
            {
                var oldValue = internalList[index];
                internalList[index] = value;
                if (!Equals(oldValue, value))
                {
                    OnPropertyChangeEvent.Invoke(internalList); 
                }
            }
        }

        public void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action) => onAdd.SafeBind(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action) => onRemove.SafeBind(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => onClear.SafeBind(destroyable, action);
    }
}
