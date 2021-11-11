using System;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveArray<T> : WriteableReactiveProperty<IEnumerable<T>>, IReactiveArray<T>
    {
        T[] internalArray;

        readonly SafeAction<(int index, T newItem, T formerItem)> onModifyItem = new SafeAction<(int,T,T)>();

        public WriteableReactiveArray(int lenght = 0) => internalArray = new T[lenght];

        public WriteableReactiveArray(IEnumerable<T> content)
        {
            if (content != null)
            {
                internalArray = content.ToArray();
            }
            else
            {
                Debug.LogError("Attempting to instantiate a ReactiveArray without content");
            }
        }
        
        public override IEnumerable<T> Value
        {
            get => internalArray.ToArray();
            set
            {
                internalArray = value.ToArray();
                OnPropertyChangeEvent.Invoke(internalArray);
            }
        }
        
        public int Lenght => internalArray.Length;

        public T this[int index]
        {
            get => internalArray[index];
            set
            {
                var oldValue = internalArray[index];
                internalArray[index] = value;
                if (!Equals(oldValue, value))
                {
                    OnPropertyChangeEvent.Invoke(internalArray);
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
