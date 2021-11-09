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
        IReactiveDictionary<TK, TV> Reader => Writer;
        
        public IEnumerable<KeyValuePair<TK, TV>> Value => Writer.Value;
        public int Count => Writer.Count;
        public TV this[TK key] => Writer[key];
        
        public ReactiveDictionary() => Writer = new WriteableReactiveDictionary<TK, TV>();
        
        public void SafeBindOnChangeAction(IDestroyable destroyable, Action<IEnumerable<KeyValuePair<TK, TV>>> action, bool invokeOnBind = true) =>
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
