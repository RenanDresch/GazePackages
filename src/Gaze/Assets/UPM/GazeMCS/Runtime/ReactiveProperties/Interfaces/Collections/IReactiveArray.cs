using System.Collections;

namespace Gaze.MCS
{
    public interface IReactiveArray<T> : IReactiveIndexable<IReactiveArray<T>, int, T>, ICollection, IReleasable, IResettable {}
}
