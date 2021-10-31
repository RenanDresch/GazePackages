using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gaze.MVVM
{
    [System.Serializable]
    public class WriteableReactiveDictionary<TK, TV> : WriteableReactiveProperty<IDictionary<TK, TV>>, IDictionary<TK, TV>
    {
        Dictionary<TK, TV> internalDictionary;

        public WriteableReactiveDictionary() => currentValue = internalDictionary = new Dictionary<TK, TV>();
        
        public ICollection<TK> Keys => internalDictionary.Keys;
        public ICollection<TV> Values => internalDictionary.Values;
        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => internalDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(KeyValuePair<TK, TV> item) => internalDictionary.Contains(item);
        public int Count => internalDictionary.Count;
        public bool IsReadOnly => false;
        public bool TryGetValue(TK key, out TV value) => internalDictionary.TryGetValue(key, out value);
        public bool ContainsKey(TK key) => internalDictionary.ContainsKey(key);
        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
        {
            var iInternalDictionary = (IDictionary)internalDictionary;
            iInternalDictionary.CopyTo(array, arrayIndex);
        }
        
        public void Add(KeyValuePair<TK, TV> item)
        {
            internalDictionary.Add(item.Key, item.Value);
            OnPropertyChangeEvent?.Invoke(internalDictionary);
        }
        public void Clear()
        {
            internalDictionary.Clear();
            OnPropertyChangeEvent?.Invoke(internalDictionary);
        }

        public bool Remove(KeyValuePair<TK, TV> item)
        {
            var result = false;
            if (Contains(item))
            {
                internalDictionary.Remove(item.Key);
                OnPropertyChangeEvent?.Invoke(internalDictionary);
                result = true;
            }
            return result;
        }
        
        public void Add(TK key, TV value)
        {
            internalDictionary.Add(key, value);
            OnPropertyChangeEvent?.Invoke(internalDictionary);
        }
      
        public bool Remove(TK key)
        {
            var result = false;
            if (ContainsKey(key))
            {
                internalDictionary.Remove(key);
                OnPropertyChangeEvent?.Invoke(internalDictionary);
                result = true;
            }
            return result;
        }


        public TV this[TK key]
        {
            get => internalDictionary[key];
            set
            {
                var oldValue = internalDictionary[key];
                internalDictionary[key] = value;
                if (!Equals(oldValue, value))
                {
                    OnPropertyChangeEvent?.Invoke(internalDictionary);
                }
            }
        }
    }
}
