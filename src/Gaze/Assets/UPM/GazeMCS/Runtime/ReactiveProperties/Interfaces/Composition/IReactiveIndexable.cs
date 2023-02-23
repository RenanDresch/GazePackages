using System;

namespace Gaze.MCS
{
    public interface IReactiveIndexable<out TI, in TK,T> : IChangeable<TI>
    {
        IReactiveProperty<T> this[TK key] { get; set; }

        TI WithCustomDefaultValueGetter(Func<T> defaultValueOverride);
    }
}