using System;
using System.Collections;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveArray<T> : ReactiveReadOnlyCollection<IReactiveArray<T>, int, T>, IReactiveArray<T>
    {
        readonly IReactiveProperty<T>[] internalArray;
        readonly IReactiveProperty<T>[] initialCollection;
        readonly SafeAction<int, IReactiveProperty<T>> onSet = new SafeAction<int, IReactiveProperty<T>>();
        readonly SafeAction<int, IReactiveProperty<T>, IReactiveProperty<T>> onReplace = new SafeAction<int, IReactiveProperty<T>, IReactiveProperty<T>>();
        
        protected override IReactiveArray<T> Builder => this;

        public override bool TryGetValue(int index, out IReactiveProperty<T> value)
        {
            value = internalArray[index];
            return value != null;
        }

        protected override IReactiveProperty<T> IndexGetter(int index) => internalArray[index];

        protected override void IndexSetter(int index, IReactiveProperty<T> value)
        {
            internalArray[index] = value;
            onSet.Invoke(index, value);
        }

        protected override void IndexReplacer(int index, IReactiveProperty<T> replacedValue, IReactiveProperty<T> newValue
        )
        {
            IndexSetter(index, newValue);
            onReplace.Invoke(index, replacedValue, newValue);
        }

        public int Count => internalArray.Length;

        public ReactiveArray(int lenght)
        {
            internalArray = new IReactiveProperty<T>[lenght];
            for (var i = 0; i < lenght; i++)
            {
                internalArray[i] = new ReactiveProperty<T>(GetDefaultValue());
            }
        }
        
        public ReactiveArray(IReactiveProperty<T>[] collection) => initialCollection = internalArray = collection;

        public bool IsSynchronized => internalArray.IsSynchronized;
        public object SyncRoot => internalArray.SyncRoot;
        
        public void CopyTo(Array array, int index) => internalArray.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => internalArray.GetEnumerator();
        public void Reset()
        {
            for (var i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] = initialCollection != null ? initialCollection[i] : new ReactiveProperty<T>(GetDefaultValue());
            }
        }

        public IReactiveArray<T> SafeBindOnSetAction(
                IDestroyable destroyable,
                Action<int, IReactiveProperty<T>> action
        )
        {
            onSet.SafeBind(
                    destroyable,
                    action);

            return this;
        }

        public IReactiveArray<T> UnbindOnSetAction(
                Action<int, IReactiveProperty<T>> action
        )
        {
            onSet.Unbind(action);

            return this;
        }

        public IReactiveArray<T> SafeBindOnReplaceAction(
                IDestroyable destroyable,
                Action<int, IReactiveProperty<T>, IReactiveProperty<T>> action
        )
        {
            onReplace.SafeBind(
                    destroyable,
                    action);

            return this;
        }

        public IReactiveArray<T> UnbindOnReplaceAction(
                Action<int, IReactiveProperty<T>, IReactiveProperty<T>> action
        )
        {
            onReplace.Unbind(action);

            return this;
        }
    }
}
