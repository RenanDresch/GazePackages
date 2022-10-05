using System;
using System.Collections.Generic;

namespace Gaze.MCS
{
    public abstract class ReactiveComparable<TI, T> : ReactiveImplementation<TI>, ICustomComparable<TI, T>
    { 
        ICustomComparable<TI, T> customComparableImplementation; 
        protected Func<T, T, bool> IsEquals { get; private set; } = (a, b) => EqualityComparer<T>.Default.Equals(a, b);

        public TI WithCustomComparer(Func<T, T, bool> customComparer)
        {
            IsEquals = customComparer;
            return Builder;
        }
    }
}