using System;
using System.Collections.Generic;
using System.Linq;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveArray<T> : IReactiveArray<T>
    {
        public readonly WriteableReactiveArray<T> Writer;
        
        public T[] Value => Writer.Value.ToArray();
        public int Lenght => Writer.Lenght;
        public T this[int index] => Writer[index];

        public ReactiveArray(int lenght = 0) => Writer = new WriteableReactiveArray<T>(lenght);

        public ReactiveArray(IEnumerable<T> content) => Writer = new WriteableReactiveArray<T>(content);

        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveArray<T> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty.Writer);
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<T[]> action, bool invokeOnBind = true) => 
            Writer.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void SafeBindOnModifyItemAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) =>
            Writer.SafeBindOnModifyItemAction(destroyable, action);
    }
}
