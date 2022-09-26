using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveDictionary<TK, TV> : IDictionary<TK, IReactiveProperty<TV>>, IReleasable
    {
        IReactiveDictionary<TK, TV> SafeBindOnAddAction(IDestroyable destroyable, Action<KeyValuePair<TK, IReactiveProperty<TV>>> action);
        IReactiveDictionary<TK, TV> SafeBindOnRemoveAction(IDestroyable destroyable, Action<KeyValuePair<TK, IReactiveProperty<TV>>> action);
        IReactiveDictionary<TK, TV> SafeBindOnReplaceAction(IDestroyable destroyable, Action<(TK key, IReactiveProperty<TV> newItem, IReactiveProperty<TV> replacedItem)> action);
        IReactiveDictionary<TK, TV> SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
