using System;

namespace Gaze.MCS
{
    public interface IReactiveIndexable<out TI, in TK,T> : IChangeable<TI>
    {
        T this[TK key] { get; set; }

        TI WithCustomDefaultValueGetter(Func<T> defaultValueOverride);
    }
}