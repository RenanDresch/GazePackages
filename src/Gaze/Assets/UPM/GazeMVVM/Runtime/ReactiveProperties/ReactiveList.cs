using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveList<T> : IReactiveList<T>
    {
        public readonly WriteableReactiveList<T> Writer;
        IReactiveList<T> Reader => Writer;
        
        public IEnumerable<T> Value => Reader.Value;
        public int Count => Reader.Count;
        public T this[int index] => Writer[index];

        public ReactiveList(IEnumerable<T> content = default) => Writer = new WriteableReactiveList<T>(content);

        public void SafeBindToReactiveProperty(IDestroyable destroyable, IReactiveProperty<IEnumerable<T>> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty);
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<T>> action, bool invokeOnBind = true) =>
            Reader.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void SafeBindOnAddAction(IDestroyable destroyable, Action<T> action) =>
            Reader.SafeBindOnAddAction(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<T> action) =>
            Reader.SafeBindOnRemoveAction(destroyable, action);
        public void SafeBindOnReplaceAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action) =>
            Reader.SafeBindOnReplaceAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) =>
            Reader.SafeBindOnClearAction(destroyable, action);
    }
}
