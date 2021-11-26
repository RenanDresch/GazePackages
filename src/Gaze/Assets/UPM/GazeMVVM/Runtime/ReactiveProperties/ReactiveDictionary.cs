using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveDictionary<TK, TV> : IReactiveDictionary<TK, TV>
    {
        public readonly WriteableReactiveDictionary<TK, TV> Writer;

        public Dictionary<TK, TV> Value => new Dictionary<TK, TV>(Writer.Value);
        public int Count => Writer.Count;
        public TV this[TK key] => Writer[key];
        
        public ReactiveDictionary() => Writer = new WriteableReactiveDictionary<TK, TV>();
        
        public void SafeBindToReactiveProperty(IDestroyable destroyable, ReactiveDictionary<TK, TV> targetReactiveProperty) =>
            Writer.SafeBindToReactiveProperty(destroyable, targetReactiveProperty.Writer);
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<Dictionary<TK, TV>> action, bool invokeOnBind = true) =>
            Writer.SafeBindOnChangeAction(destroyable, action, invokeOnBind);
        public void SafeBindOnAddAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action) =>
            Writer.SafeBindOnAddAction(destroyable, action);
        public void SafeBindOnRemoveAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>> action) =>
            Writer.SafeBindOnRemoveAction(destroyable, action);
        public void SafeBindOnReplaceAction(IDestroyable destroyable, Action<KeyValuePair<TK, TV>, TV> action) =>
            Writer.SafeBindOnReplaceAction(destroyable, action);
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) =>
            Writer.SafeBindOnClearAction(destroyable, action);
    }
}
