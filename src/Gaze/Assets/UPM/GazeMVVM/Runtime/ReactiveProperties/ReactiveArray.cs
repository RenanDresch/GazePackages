using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveArray<T> : IReactiveArray<T>
    {
        public readonly WriteableReactiveArray<T> Writer;
        IReactiveArray<T> Reader => Writer;
        
        public IEnumerable<T> Value => Reader.Value;
        public int Lenght => Reader.Lenght;
        public T this[int index] => Writer[index];

        public ReactiveArray(int lenght = 0) => Writer = new WriteableReactiveArray<T>(lenght);

        public ReactiveArray(IEnumerable<T> content) => Writer = new WriteableReactiveArray<T>(content);

        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<T>> action, bool invokeOnBind = true) => 
            Reader.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void SafeBindOnModifyItemAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) =>
            Reader.SafeBindOnModifyItemAction(destroyable, action);
    }
}
