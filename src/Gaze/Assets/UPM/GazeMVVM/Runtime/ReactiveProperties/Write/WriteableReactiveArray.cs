using System;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveArray<T> : WriteableReactiveProperty<T[]>, IReactiveArray<T>
    {
        readonly SafeAction<(int index, T newItem, T formerItem)> onModifyItem = new SafeAction<(int,T,T)>();

        public WriteableReactiveArray(int lenght = 0) => Value = new T[lenght];

        public WriteableReactiveArray(IEnumerable<T> content)
        {
            if (content != null)
            {
                Value = content.ToArray();
            }
            else
            {
                Debug.LogError("Attempting to instantiate a ReactiveArray without content");
            }
        }
        
        public int Lenght => Value.Length;

        public T this[int index]
        {
            get => Value[index];
            set
            {
                var oldValue = Value[index];
                Value[index] = value;
                if (!Equals(oldValue, value))
                {
                    OnPropertyChangeEvent.Invoke(Value);
                    onModifyItem.Invoke((index, value, oldValue));
                }
            }
        }
        
        public void SafeBindOnModifyItemAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) => onModifyItem.SafeBind(destroyable, action);

        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public void UnbindAllActions()
        {
            OnPropertyChangeEvent.UnbindAll();
            onModifyItem.UnbindAll();
        }
    }
}
