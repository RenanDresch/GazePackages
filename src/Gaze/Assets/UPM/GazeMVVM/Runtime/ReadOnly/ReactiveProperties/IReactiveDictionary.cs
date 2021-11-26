using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveDictionary<TK, TV> : IReactiveProperty<Dictionary<TK, TV>>
    {
        int Count { get; }
        TV this[TK key] { get; }
        void SafeBindOnAddAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action);
        void SafeBindOnRemoveAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action);
        void SafeBindOnReplaceAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>, TV> action);
        void SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}
