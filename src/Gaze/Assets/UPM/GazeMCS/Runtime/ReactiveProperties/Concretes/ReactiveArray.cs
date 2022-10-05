using System;
using System.Collections;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveArray<T> : ReactiveReadOnlyCollection<IReactiveArray<T>, int, T>, IReactiveArray<T>
    {
        readonly IReactiveProperty<T>[] internalArray;
        
        protected override IReactiveArray<T> Builder => this;

        public override bool TryGetValue(int index, out IReactiveProperty<T> value)
        {
            value = internalArray[index];
            return value != null;
        }

        protected override IReactiveProperty<T> IndexGetter(int index) => internalArray[index];
        
        protected override void IndexSetter(int index, IReactiveProperty<T> value) => internalArray[index] = value;
        
        public int Count => internalArray.Length;

        public ReactiveArray(int lenght)
        {
            internalArray = new IReactiveProperty<T>[lenght];
            for (var i = 0; i < lenght; i++)
            {
                internalArray[i] = new ReactiveProperty<T>(GetDefaultValue());
            }
        }
        
        public ReactiveArray(IReactiveProperty<T>[] collection) => internalArray = collection;

        public bool IsSynchronized => internalArray.IsSynchronized;
        public object SyncRoot => internalArray.SyncRoot;
        
        public void CopyTo(Array array, int index) => internalArray.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => internalArray.GetEnumerator();
    }
}
