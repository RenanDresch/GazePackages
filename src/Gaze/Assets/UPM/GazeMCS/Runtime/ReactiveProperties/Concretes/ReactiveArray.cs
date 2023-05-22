using System;
using System.Collections;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveArray<T> : ReactiveReadOnlyCollection<IReactiveArray<T>, int, T>, IReactiveArray<T>
    {
        readonly T[] internalArray;
        readonly SafeAction<int, T> onSet = new SafeAction<int, T>();
        readonly SafeAction<int, T, T> onReplace = new SafeAction<int, T, T>();
        
        protected override IReactiveArray<T> Builder => this;

        public override bool TryGetValue(int index, out T value)
        {
            value = internalArray[index];
            return value != null;
        }

        protected override T IndexGetter(int index) => internalArray[index];

        protected override void IndexSetter(int index, T value)
        {
            internalArray[index] = value;
            onSet.Invoke(index, value);
        }

        protected override void IndexReplacer(int index, T replacedValue, T newValue
        )
        {
            IndexSetter(index, newValue);
            onReplace.Invoke(index, replacedValue, newValue);
        }

        public int Count => internalArray.Length;

        public ReactiveArray(int lenght)
        {
            internalArray = new T[lenght];
            for (var i = 0; i < lenght; i++)
            {
                internalArray[i] = new ReactiveProperty<T>(GetDefaultValue());
            }
        }
        
        public ReactiveArray(T[] collection) => internalArray = collection;

        public bool IsSynchronized => internalArray.IsSynchronized;
        public object SyncRoot => internalArray.SyncRoot;
        
        public void CopyTo(Array array, int index) => internalArray.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => internalArray.GetEnumerator();
        public void Reset()
        {
            for (var i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] = GetDefaultValue();
            }
        }

        public IReactiveArray<T> SafeBindOnSetAction(
                IDestroyable destroyable,
                Action<int, T> action
        )
        {
            onSet.SafeBind(
                    destroyable,
                    action);

            return this;
        }

        public IReactiveArray<T> UnbindOnSetAction(
                Action<int, T> action
        )
        {
            onSet.Unbind(action);

            return this;
        }

        public IReactiveArray<T> SafeBindOnReplaceAction(
                IDestroyable destroyable,
                Action<int, T, T> action
        )
        {
            onReplace.SafeBind(
                    destroyable,
                    action);

            return this;
        }

        public IReactiveArray<T> UnbindOnReplaceAction(
                Action<int, T, T> action
        )
        {
            onReplace.Unbind(action);

            return this;
        }
    }
}
