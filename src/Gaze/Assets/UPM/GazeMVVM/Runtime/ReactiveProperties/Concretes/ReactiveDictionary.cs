using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveDictionary<TK, TV> : IReactiveDictionary<TK, TV>
    {
        readonly Dictionary<TK, IReactiveProperty<TV>> internalDictionary;

        readonly SafeAction<IReactiveDictionary<TK, TV>> onChange = new SafeAction<IReactiveDictionary<TK, TV>>();
        readonly SafeAction<KeyValuePair<TK, IReactiveProperty<TV>>> onAdd = new SafeAction<KeyValuePair<TK, IReactiveProperty<TV>>>();
        readonly SafeAction<KeyValuePair<TK, IReactiveProperty<TV>>> onRemove = new SafeAction<KeyValuePair<TK, IReactiveProperty<TV>>>();
        readonly SafeAction<(TK key, IReactiveProperty<TV> newItem, IReactiveProperty<TV> replacedItem)> onReplace = new SafeAction<(TK key, IReactiveProperty<TV> newItem, IReactiveProperty<TV> replacedItem)>();
        readonly SafeAction onClear = new SafeAction();

        
        /// <summary>
        /// Instantiates a ReactiveDictionary with the required capacity.
        /// </summary>
        /// <param name="capacity">Internal Dictionary capacity.</param>
        public ReactiveDictionary(int capacity) =>
            internalDictionary = new Dictionary<TK, IReactiveProperty<TV>>(capacity);
        
        /// <summary>
        /// Instantiates a ReactiveDictionary passing an existing collection.
        /// </summary>
        /// <param name="collection">Collection to be used internally.</param>
        public ReactiveDictionary(Dictionary<TK, IReactiveProperty<TV>> collection) => internalDictionary = collection;
        
        public IReactiveProperty<TV> this[TK key]
        {
            get => internalDictionary[key];
            set
            {
                var oldValue = internalDictionary[key];
                internalDictionary[key] = value;
                if (!Equals(oldValue, value))
                {
                    onChange.Invoke(this);
                    onReplace.Invoke((key, value, oldValue));
                }
            }
        }
        
        /// <summary>
        /// Count the internal Dictionary.
        /// </summary>
        public int Count => internalDictionary.Count;
      
       
        public void Add(KeyValuePair<TK, IReactiveProperty<TV>> item)
        {
            internalDictionary.Add(item.Key, item.Value);
            onChange.Invoke(this);
            onAdd.Invoke(item);
        }
        
        public bool Remove(KeyValuePair<TK, IReactiveProperty<TV>> item)
        {
            var result = false;
            if (Contains(item))
            {
                internalDictionary.Remove(item.Key);
                onChange.Invoke(this);
                onRemove.Invoke(item);
                result = true;
            }
            return result;
        }
        
        public void Clear()
        {
            internalDictionary.Clear();
            onChange.Invoke(this);
            onClear.Invoke();
        }
        
        public void Add(TK key, IReactiveProperty<TV> value)
        {
            internalDictionary.Add(key, value);
            onChange.Invoke(this);
            onAdd.Invoke(new KeyValuePair<TK, IReactiveProperty<TV>>(key, value));
        }
        
        public bool Remove(TK key)
        {
            var result = false;
            if (ContainsKey(key))
            {
                var value = internalDictionary[key];
                internalDictionary.Remove(key);
                onChange.Invoke(this);
                onRemove.Invoke(new KeyValuePair<TK, IReactiveProperty<TV>>(key, value));
                result = true;
            }
            return result;
        }

        public IReactiveDictionary<TK, TV> SafeBindOnChangeAction(IDestroyable destroyable,
            Action<IReactiveDictionary<TK, TV>> action)
        {
            onChange.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveDictionary<TK, TV> SafeBindOnAddAction(IDestroyable destroyable,
            Action<KeyValuePair<TK, IReactiveProperty<TV>>> action)
        {
            onAdd.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveDictionary<TK, TV> SafeBindOnRemoveAction(IDestroyable destroyable,
            Action<KeyValuePair<TK, IReactiveProperty<TV>>> action)
        {
            onRemove.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveDictionary<TK, TV> SafeBindOnReplaceAction(IDestroyable destroyable,
            Action<(TK key, IReactiveProperty<TV> newItem, IReactiveProperty<TV> replacedItem)> action)
        {
            onReplace.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveDictionary<TK, TV> SafeBindOnClearAction(IDestroyable destroyable, Action action)
        {
            onClear.SafeBind(destroyable, action);
            return this;
        }
        
        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public void Release()
        {
            onChange.Release();
            onAdd.Release();
            onRemove.Release();
            onReplace.Release();
            onClear.Release();
        }
        
        public void CopyTo(KeyValuePair<TK, IReactiveProperty<TV>>[] array, int arrayIndex)
        {
            var iInternalDictionary = (IDictionary)internalDictionary;
            iInternalDictionary.CopyTo(array, arrayIndex);
        }
        public bool ContainsKey(TK key) => internalDictionary.ContainsKey(key);
        public bool TryGetValue(TK key, out IReactiveProperty<TV> value) => internalDictionary.TryGetValue(key, out value);
        public bool IsReadOnly => false;
        public bool Contains(KeyValuePair<TK, IReactiveProperty<TV>> item) => internalDictionary.Contains(item);
        public IEnumerator<KeyValuePair<TK, IReactiveProperty<TV>>> GetEnumerator() => internalDictionary.GetEnumerator();
        public ICollection<TK> Keys => internalDictionary.Keys;
        public ICollection<IReactiveProperty<TV>> Values => internalDictionary.Values;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
