using System.Collections.Generic;

namespace Gaze.MCS
{
    public interface IReactiveDictionary<TK, TV> :
        ICustomComparable<IReactiveDictionary<TK, TV>, TV>,
        IReactiveCollection<IReactiveDictionary<TK, TV>, TK, TV, (TK key, TV value), TK>,
        IEnumerable<KeyValuePair<TK,IReactiveProperty<TV>>>,
        IReleasable, IResettable
    {
        public void Add(TK key, TV value);
    }
}
