using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveArray<T> : ReactiveProperty<T[]>, IReactiveArray<T>
    {
        readonly SafeAction<(int index, T newItem, T formerItem)> onModifyItem = new SafeAction<(int,T,T)>();

        readonly Func<T, T, bool> valueComparer;
        
        public IEnumerator Enumerator { get; private set; }

        ReactiveArray(Func<T, T, bool> valueComparer)
        {
            this.valueComparer = valueComparer ?? ((a, b) => Equals(a,b));
        }

        public ReactiveArray(int lenght = 0, Func<T, T, bool> valueComparer = null) : this(valueComparer)
        {
            Value = new T[lenght];
            CacheEnumerator();
        }

        public ReactiveArray(IEnumerable<T> content, Func<T, T, bool> valueComparer = null) : this(valueComparer)
        {
            Value = content.ToArray();
        }
        
        public int Lenght => Value.Length;

        public T this[int index]
        {
            get => Value[index];
            set
            {
                var oldValue = Value[index];
                Value[index] = value;
                if (!valueComparer(oldValue, value))
                {
                    CacheEnumerator();
                    
                    OnPropertyChangeEvent.Invoke(Value);
                    onModifyItem.Invoke((index, value, oldValue));
                }
            }
        }
        
        public void SafeBindOnModifyItemAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) => onModifyItem.SafeBind(destroyable, action);

        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public override void Release()
        {
            base.Release();
            onModifyItem.UnbindAll();
        }
        
        void CacheEnumerator()
        {
            Enumerator = Value.GetEnumerator();
        }
    }
}
