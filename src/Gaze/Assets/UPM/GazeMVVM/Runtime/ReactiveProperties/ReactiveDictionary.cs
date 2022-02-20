using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveDictionary<TK, TV> : ReactiveProperty<Dictionary<TK, TV>>, IReactiveDictionary<TK, TV>, IDictionary<TK, TV>
    {
        private readonly Func<TV> instantiator = () => default;
        
        readonly SafeAction<KeyValuePair<TK, TV>> onAdd = new SafeAction<KeyValuePair<TK, TV>>();
        readonly SafeAction<KeyValuePair<TK, TV>> onRemove = new SafeAction<KeyValuePair<TK, TV>>();
        readonly SafeAction<KeyValuePair<TK, TV>, TV> onReplace = new SafeAction<KeyValuePair<TK, TV>, TV>();
        readonly SafeAction onClear = new SafeAction();

        readonly Func<TV, TV, bool> valueComparer;

        public IEnumerator<KeyValuePair<TK, TV>> Enumerator { get; private set; } = EmptyEnumerator();

        public ReactiveDictionary(Func<TV, TV, bool> valueComparer = null)
        {
            Value = new Dictionary<TK, TV>();
            this.valueComparer = valueComparer ?? ((a, b) => Equals(a,b));
            CacheEnumerator();
        }

        public ReactiveDictionary(Func<TV> instantiator, Func<TV, TV, bool> valueComparer = null) : this(valueComparer)
        {
            this.instantiator = instantiator;
        }
        
        public ICollection<TK> Keys => Value.Keys;
        public ICollection<TV> Values => Value.Values;
        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => Enumerator;
        IEnumerator IEnumerable.GetEnumerator() => Enumerator;
        public bool Contains(KeyValuePair<TK, TV> item) => Value.Contains(item);
        public int Count => Value.Count;
        public bool IsReadOnly => false;
        public bool TryGetValue(TK key, out TV value) => Value.TryGetValue(key, out value);
        public bool ContainsKey(TK key) => Value.ContainsKey(key);
        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
        {
            var iInternalDictionary = (IDictionary)Value;
            iInternalDictionary.CopyTo(array, arrayIndex);
        }
        
        public void Add(KeyValuePair<TK, TV> item)
        {
            Value.Add(item.Key, item.Value);
            OnPropertyChangeEvent.Invoke(Value);
            onAdd.Invoke(item);
            
            CacheEnumerator();
        }

        public bool Remove(KeyValuePair<TK, TV> item)
        {
            var result = false;
            if (Contains(item))
            {
                Value.Remove(item.Key);
                
                CacheEnumerator();
                
                OnPropertyChangeEvent.Invoke(Value);
                onRemove.Invoke(item);
                result = true;
            }
            
            return result;
        }
        
        public void Clear()
        {
            Value.Clear();
            
            CacheEnumerator();
            
            OnPropertyChangeEvent.Invoke(Value);
            onClear.Invoke();
        }

        public void Add(TK key, TV value)
        {
            Value.Add(key, value);
            
            CacheEnumerator();
            
            OnPropertyChangeEvent.Invoke(Value);
            onAdd.Invoke(new KeyValuePair<TK, TV>(key, value));
        }
      
        public bool Remove(TK key)
        {
            var result = false;
            if (ContainsKey(key))
            {
                var value = Value[key];
                Value.Remove(key);
                
                CacheEnumerator();
                
                OnPropertyChangeEvent.Invoke(Value);
                onRemove.Invoke(new KeyValuePair<TK, TV>(key, value));
                result = true;
            }
            return result;
        }

        public void SafeBindOnAddAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action) => onAdd.SafeBind(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action) => onRemove.SafeBind(destroyable, action);
        public void SafeBindOnReplaceAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>, TV> action) => onReplace.SafeBind(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => onClear.SafeBind(destroyable, action);
        
        public TV this[TK key]
        {
            get
            {
                if (!TryGetValue(key, out var value))
                {
                    value = instantiator();
                    Add(key, value);
                }

                return value;
            }
            set
            {
                var oldValue = ContainsKey(key) ? Value[key] : default;
                Value[key] = value;
                if (!valueComparer(oldValue, value))
                {
                    CacheEnumerator();
                    
                    OnPropertyChangeEvent.Invoke(Value);
                    onReplace.Invoke(new KeyValuePair<TK, TV>(key, value), oldValue);
                }
            }
        }

        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public override void Release()
        {
            base.Release();
            onAdd.UnbindAll();
            onRemove.UnbindAll();
            onReplace.UnbindAll();
            onClear.UnbindAll();
            Enumerator.Dispose();
        }
        
        void CacheEnumerator()
        {
            Enumerator.Dispose();
            Enumerator = Value.GetEnumerator();
        }

        //Allow null check skip
        static IEnumerator<KeyValuePair<TK, TV>> EmptyEnumerator()
        {
            yield break;;
        }
    }
}
