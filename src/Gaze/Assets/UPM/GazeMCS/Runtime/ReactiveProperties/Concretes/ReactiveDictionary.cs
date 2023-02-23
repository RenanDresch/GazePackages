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
        readonly Dictionary<TK, IReactiveProperty<TV>> internalDictionary;
        readonly Dictionary<TK, IReactiveProperty<TV>> initialCollection;

        bool cachingKeys;
        int initialCapacity;
        List<TK> keysCache; 

        public ReactiveDictionary(int capacity)
        {
            initialCapacity = capacity;
            internalDictionary = new Dictionary<TK, IReactiveProperty<TV>>(capacity);
        }


        public ReactiveDictionary(Dictionary<TK, IReactiveProperty<TV>> collection)
        {
            initialCapacity = collection.Count;
            initialCollection = internalDictionary = collection;
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
        
        public override bool TryGetValue(TK index, out IReactiveProperty<TV> value) =>
            internalDictionary.TryGetValue(index, out value);
        protected override IReactiveProperty<TV> IndexGetter(TK index) => internalDictionary[index];

        protected override void IndexSetter(TK index, IReactiveProperty<TV> value)
        {
            if (cachingKeys && !internalDictionary.ContainsKey(index))
            {
                keysCache.Add(index);
            }
            internalDictionary[index] = value;
        }

        public override int Count => internalDictionary.Count;

        public void Add(TK key, TV value) => AddToCollection((key, value));

        protected override (TK key, IReactiveProperty<TV> value) AddToCollection((TK key, TV value) item)
        {
            var reactiveValue = new ReactiveProperty<TV>(item.value);

            if (cachingKeys)
            {
                keysCache.Add(item.key);
            }
           
            internalDictionary.Add(item.key, reactiveValue);
            OnCreateIndex(item.key, reactiveValue);
            return (item.key, reactiveValue);
        }

        protected override (bool, TK key, IReactiveProperty<TV> value) RemoveFromCollection(TK key)
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
            foreach (var reactiveProperty in internalDictionary.Values)
            {
                if (reactiveProperty.CurrentValueIs(item))
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

        public bool Contains(KeyValuePair<TK, IReactiveProperty<TV>> item) => internalDictionary.Contains(item);

        public bool ContainsKey(TK key) => internalDictionary.ContainsKey(key);
        
        public List<TK> Keys => keysCache;
        
        public ICollection<IReactiveProperty<TV>> Values => internalDictionary.Values;

        public void Reset()
        {
            ClearCollection();
            
            if (initialCollection != null)
            {
                foreach (var (key, value) in initialCollection)
                {
                    internalDictionary.Add(key, value);
                }
            }
        }
    }
}
