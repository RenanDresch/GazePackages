using System;
using System.Collections;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveList<T> : ReactiveProperty<List<T>>, IReactiveList<T>, IList<T>
    {
        readonly SafeAction<T> onAdd = new SafeAction<T>();
        readonly SafeAction<T> onRemove = new SafeAction<T>();
        readonly SafeAction<(int index, T newItem, T formerItem)> onReplace = new SafeAction<(int,T,T)>();
        readonly SafeAction onClear = new SafeAction();

        readonly Func<T, T, bool> valueComparer;

        public ReactiveList(int capacity, IEnumerable<T> content = null, Func<T, T, bool> valueComparer = null)
        {
            Value = new List<T>(capacity);
            if (content != null)
            {
                foreach (var t in content)
                {
                    Value.Add(t);
                }
            }
            this.valueComparer = valueComparer ?? ((a, b) => Equals(a,b)); 
        }

        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => Value.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => Value.IndexOf(item);
        public bool Contains(T item) => Value.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Value.CopyTo(array, arrayIndex);
        
        public void Add(T item)
        {
            Value.Add(item);
            OnPropertyChangeEvent.Invoke(Value);
            onAdd.Invoke(item);
        }
        public void Clear()
        {
            Value.Clear();
            OnPropertyChangeEvent.Invoke(Value);
            onClear.Invoke();
        }
        public bool Remove(T item)
        {
            var result = Value.Remove(item);
            if (result)
            {
                OnPropertyChangeEvent.Invoke(Value);
                onRemove.Invoke(item);
            }
            return result;
        }
        public void Insert(int index, T item)
        {
            Value.Insert(index, item);
            OnPropertyChangeEvent.Invoke(Value);
            onAdd.Invoke(item);
        }
        public void RemoveAt(int index)
        {
            var removedItem = Value[index];
            Value.RemoveAt(index);
            OnPropertyChangeEvent.Invoke(Value);
            onRemove.Invoke(removedItem);
        }

        public T this[int index]
        {
            get => Value[index];
            set
            {
                var oldValue = Value[index];
                Value[index] = value;
                if (!valueComparer(oldValue, value))
                {
                    OnPropertyChangeEvent.Invoke(Value);
                    onReplace.Invoke((index, value, oldValue));
                }
            }
        }

        void OnBoundCollectionChange(IEnumerable<T> collection)
        {
            Value = new List<T>(collection);
        }

        public void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action) => onAdd.SafeBind(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action) => onRemove.SafeBind(destroyable, action);
        public void SafeBindOnReplaceAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) => onReplace.SafeBind(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => onClear.SafeBind(destroyable, action);

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
        }
    }
}
