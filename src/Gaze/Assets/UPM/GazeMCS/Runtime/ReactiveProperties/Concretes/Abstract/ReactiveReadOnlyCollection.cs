using System;

namespace Gaze.MCS
{
    public abstract class ReactiveReadOnlyCollection<TI, TK, T> :
        ReactiveComparable<TI, T>,
        IReactiveIndexable<TI, TK, T>
    {
        protected Func<T> GetDefaultValue { get; set; }

        protected ReactiveReadOnlyCollection() => GetDefaultValue = DefaultValueGetter;

        public T this[TK index]
        {
            get
            {
                if (TryGetValue(index, out var value))
                {
                    IndexGetter(index);
                }
                else
                {
                    OnCreateIndex(index, value);
                }

                return value;
            }
            set
            {
                if (TryGetValue(index, out var existingValue))
                {
                    IndexReplacer(index, existingValue, value);
                }
                else
                {
                    OnCreateIndex(index, value);
                }
            }
        }

        static T DefaultValueGetter() => default;

        public TI WithCustomDefaultValueGetter(Func<T> valueGetterOverride)
        {
            GetDefaultValue = valueGetterOverride;
            return Builder;
        }

        public abstract bool TryGetValue(TK index, out T value);
        protected abstract T IndexGetter(TK index);
        protected virtual void OnCreateIndex(TK index, T value) => IndexSetter(index, value);
        protected abstract void IndexSetter(TK index, T value);
        protected abstract void IndexReplacer(TK index, T replacedValue, T newValue);
    }
}