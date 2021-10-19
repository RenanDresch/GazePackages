using System.Collections;
using System.Collections.Generic;

namespace Gaze.MVVM
{
    [System.Serializable]
    public class ReactiveList<T> : ReactiveProperty<IEnumerable<T>>, IList<T>
    {
        List<T> internalList;

        public ReactiveList(IEnumerable<T> content = null)
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
            OnPropertyChangeEvent?.Invoke(internalList);
        }
        public void Clear()
        {
            internalList.Clear();
            OnPropertyChangeEvent?.Invoke(internalList);
        }
        public bool Remove(T item)
        {
            var result = internalList.Remove(item);
            if (result)
            {
                OnPropertyChangeEvent?.Invoke(internalList);
            }
            return result;
        }
        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            OnPropertyChangeEvent?.Invoke(internalList);
        }
        public void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
            OnPropertyChangeEvent?.Invoke(internalList);
        }

        public T this[int index]
        {
            get => internalList[index];
            set
            {
                internalList[index] = value;
                OnPropertyChangeEvent?.Invoke(internalList);
            }
        }
    }
}
