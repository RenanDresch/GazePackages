using System.Collections.Generic;

namespace Gaze.MCS
{
    public interface IReactiveDictionary<TK, TV> :
        ICustomComparable<IReactiveDictionary<TK, TV>, TV>,
        IReactiveCollection<IReactiveDictionary<TK, TV>, TK, TV, (TK key, TV value), TK>,
        IReleasable, IResettable
    {
        List<TK> Keys { get; }
        IReactiveDictionary<TK, TV> WithKeyCaching();
        void Add(TK key, TV value);
    }
}
