using System;
using System.Collections;
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

        public ReactiveDictionary(int capacity) =>
            internalDictionary = new Dictionary<TK, IReactiveProperty<TV>>(capacity);
        
        public ReactiveDictionary(Dictionary<TK, IReactiveProperty<TV>> collection) => internalDictionary = collection;
        
        protected override IReactiveDictionary<TK, TV> Builder => this;
        
        public override bool TryGetValue(TK index, out IReactiveProperty<TV> value) =>
            internalDictionary.TryGetValue(index, out value);
        protected override IReactiveProperty<TV> IndexGetter(TK index) => internalDictionary[index];
        
        protected override void IndexSetter(TK index, IReactiveProperty<TV> value) => internalDictionary[index] = value;

        public override int Count => internalDictionary.Count;

        public void Add(TK key, TV value) => AddToCollection((key, value));

        protected override (TK key, IReactiveProperty<TV> value) AddToCollection((TK key, TV value) item)
        {
            var reactiveValue = new ReactiveProperty<TV>(item.value);
            internalDictionary.Add(item.key, reactiveValue);
            OnCreateIndex(item.key, reactiveValue);
            return (item.key, reactiveValue);
        }

        protected override (bool, TK key, IReactiveProperty<TV> value) RemoveFromCollection(TK key)
        {
            var success = TryGetValue(key, out var value);
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

        protected override void ClearCollection() => internalDictionary.Clear();

        public bool Contains(KeyValuePair<TK, IReactiveProperty<TV>> item) => internalDictionary.Contains(item);
        
        public ICollection<TK> Keys => internalDictionary.Keys;
        
        public ICollection<IReactiveProperty<TV>> Values => internalDictionary.Values;
        public IEnumerator<KeyValuePair<TK, IReactiveProperty<TV>>> GetEnumerator() => internalDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
