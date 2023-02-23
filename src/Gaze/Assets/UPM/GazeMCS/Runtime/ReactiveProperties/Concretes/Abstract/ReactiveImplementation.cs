using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public abstract class ReactiveImplementation<TI> : BuilderImplementation<TI>, IChangeable<TI>, IReleasable
    {
        //Todo: implement this, currently, this leads nowhere...
        protected readonly SafeAction<TI> OnChange = new SafeAction<TI>();
        public TI SafeBindOnChangeAction(IDestroyable destroyable, Action<TI> action)
        {
            OnChange.SafeBind(destroyable, action);
            return Builder;
        }

        public TI UnbindOnChangeAction(Action<TI> action)
        {
            OnChange.Unbind(action);
            return Builder;
        }

        public virtual void Release()
        {
            OnChange.Release();
        }
    }
}