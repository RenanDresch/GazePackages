using System;
using System.Collections.Generic;
using System.Linq;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveDictionary<TK, TV> :
        ReactiveCollection<IReactiveDictionary<TK, TV>, TK, TV, (TK key,TV value), TK>,
        IReactiveDictionary<TK, TV>
    {
        readonly Dictionary<TK, TV> internalDictionary;
        
        bool cachingKeys;
        int initialCapacity;
        List<TK> keysCache;

        public ReactiveDictionary(int capacity)
        {
            initialCapacity = capacity;
            internalDictionary = new Dictionary<TK, TV>(capacity);
        }

        public ReactiveDictionary(Dictionary<TK, TV> collection)
        {
            initialCapacity = collection.Count;
        }

        public IReactiveDictionary<TK, TV> WithKeyCaching()
        {
            cachingKeys = true;
            keysCache = new List<TK>(internalDictionary.Count > initialCapacity ? internalDictionary.Count : initialCapacity);
            foreach (var key in internalDictionary.Keys)
            {
                keysCache.Add(key);
            }

            return this;
        }
        
        protected override IReactiveDictionary<TK, TV> Builder => this;
        
        public override bool TryGetValue(TK index, out TV value) =>
            internalDictionary.TryGetValue(index, out value);
        
        protected override TV IndexGetter(TK index) => internalDictionary[index];

        protected override void IndexSetter(TK index, TV value)
        {
            if (cachingKeys && !internalDictionary.ContainsKey(index))
            {
                keysCache.Add(index);
            }
            internalDictionary[index] = value;
        }

        public override int Count => internalDictionary.Count;

        public void Add(TK key, TV value) => AddToCollection((key, value));

        protected override (TK key, TV value) AddToCollection((TK key, TV value) item)
        {
            if (cachingKeys)
            {
                keysCache.Add(item.key);
            }
           
            internalDictionary.Add(item.key, item.value);
            OnCreateIndex(item.key, item.value);
            return (item.key, item.value);
        }

        protected override (bool, TK key, TV value) RemoveFromCollection(TK key)
        {
            var success = TryGetValue(key, out var value);

            if (cachingKeys)
            {
                keysCache.Remove(key);    
            }
            
            internalDictionary.Remove(key);
            return (success, key, value);
        }

        public override bool Contains(TV item)
        {
            foreach (var value in internalDictionary.Values)
            {
                if (EqualityComparer<TV>.Default.Equals(value, item))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void ClearCollection()
        {
            internalDictionary.Clear();

            if (cachingKeys)
            {
                keysCache.Clear();
            }
        }

        public bool Contains(KeyValuePair<TK, TV> item) => internalDictionary.Contains(item);

        public bool ContainsKey(TK key) => internalDictionary.ContainsKey(key);
        
        public List<TK> Keys => keysCache;
        
        public ICollection<TV> Values => internalDictionary.Values;

        public void Reset()
        {
            ClearCollection();
        }
    }
}
