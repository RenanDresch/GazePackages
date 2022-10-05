using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public abstract class ReactiveReplaceable<TI, TK, T> : ReactiveComparable<TI, T>//, IReplaceable<TI, TK, T>
    {
        protected readonly SafeAction<(TK index, T newItem, T replacedItem)>
            OnReplace = new SafeAction<(TK index, T newItem, T replacedItem)>();
        
        public TI SafeBindOnReplaceItemAction(IDestroyable destroyable, Action<(TK index, T newItem, T replacedItem)> action)
        {
            OnReplace.SafeBind(destroyable, action);
            return Builder;
        }

        public override void Release()
        {
            base.Release();
            OnReplace.Release();
        }
    }
}