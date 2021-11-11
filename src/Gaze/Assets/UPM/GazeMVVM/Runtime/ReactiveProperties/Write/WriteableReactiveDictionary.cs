using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [System.Serializable]
    public class WriteableReactiveDictionary<TK, TV> : WriteableReactiveProperty<IEnumerable<KeyValuePair<TK, TV>>>, IDictionary<TK, TV>, IReactiveDictionary<TK, TV>
    {
        Dictionary<TK, TV> internalDictionary;

        readonly SafeAction<KeyValuePair<TK, TV>> onAdd = new SafeAction<KeyValuePair<TK, TV>>();
        readonly SafeAction<KeyValuePair<TK, TV>> onRemove = new SafeAction<KeyValuePair<TK, TV>>();
        readonly SafeAction<KeyValuePair<TK, TV>, TV> onReplace = new SafeAction<KeyValuePair<TK, TV>, TV>();
        readonly SafeAction onClear = new SafeAction();
        
        public WriteableReactiveDictionary() => internalDictionary = new Dictionary<TK, TV>();

        public override IEnumerable<KeyValuePair<TK, TV>> Value
        {
            get => internalDictionary.ToArray();
            set
            {
                internalDictionary = value.ToDictionary(pair => pair.Key, pair => pair.Value);
                OnPropertyChangeEvent.Invoke(internalDictionary);
            }
        }

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
            OnPropertyChangeEvent.Invoke(internalDictionary);
            onAdd.Invoke(item);
        }
        
        public bool Remove(KeyValuePair<TK, TV> item)
        {
            var result = false;
            if (Contains(item))
            {
                internalDictionary.Remove(item.Key);
                OnPropertyChangeEvent.Invoke(internalDictionary);
                onRemove.Invoke(item);
                result = true;
            }
            return result;
        }
        
        public void Clear()
        {
            internalDictionary.Clear();
            OnPropertyChangeEvent.Invoke(internalDictionary);
            onClear.Invoke();
        }

        public void Add(TK key, TV value)
        {
            internalDictionary.Add(key, value);
            OnPropertyChangeEvent.Invoke(internalDictionary);
            onAdd.Invoke(new KeyValuePair<TK, TV>(key, value));
        }
      
        public bool Remove(TK key)
        {
            var result = false;
            if (ContainsKey(key))
            {
                var value = internalDictionary[key];
                internalDictionary.Remove(key);
                OnPropertyChangeEvent.Invoke(internalDictionary);
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
            get => internalDictionary[key];
            set
            {
                var oldValue = internalDictionary[key];
                internalDictionary[key] = value;
                if (!Equals(oldValue, value))
                {
                    OnPropertyChangeEvent.Invoke(internalDictionary);
                    onReplace.Invoke(new KeyValuePair<TK, TV>(key, value), oldValue);
                }
            }
        }
        
        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public void UnbindAllActions()
        {
            OnPropertyChangeEvent.UnbindAll();
            onAdd.UnbindAll();
            onRemove.UnbindAll();
            onReplace.UnbindAll();
            onClear.UnbindAll();
        }
    }
}
