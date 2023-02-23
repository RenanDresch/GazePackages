using System;

namespace Gaze.MCS
{
    public abstract class ReactiveReadOnlyCollection<TI, TK, T> :
        ReactiveComparable<TI, T>,
        IReactiveIndexable<TI, TK, T>
    {
        protected Func<T> GetDefaultValue { get; set; }

        protected ReactiveReadOnlyCollection() => GetDefaultValue = DefaultValueGetter;

        public IReactiveProperty<T> this[TK index]
        {
            get
            {
                if (TryGetValue(index, out var value))
                {
                    IndexGetter(index);
                }
                else
                {
                    value = new ReactiveProperty<T>(GetDefaultValue());
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

        public abstract bool TryGetValue(TK index, out IReactiveProperty<T> value);
        protected abstract IReactiveProperty<T> IndexGetter(TK index);
        protected virtual void OnCreateIndex(TK index, IReactiveProperty<T> value) => IndexSetter(index, value);
        protected abstract void IndexSetter(TK index, IReactiveProperty<T> value);
        protected abstract void IndexReplacer(TK index, IReactiveProperty<T> replacedValue, IReactiveProperty<T> newValue);
    }
}