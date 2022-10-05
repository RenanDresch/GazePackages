using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public abstract class ReactiveCollection<TI, TK, T, TA, TR> :
        ReactiveReadOnlyCollection<TI, TK, T>,
        IReactiveCollection<TI, TK, T, TA, TR>
    {
        readonly SafeAction<TK, IReactiveProperty<T>> onAdd = new SafeAction<TK, IReactiveProperty<T>>();
        readonly SafeAction<TK, IReactiveProperty<T>> onRemove = new SafeAction<TK, IReactiveProperty<T>>();
        readonly SafeAction onClear = new SafeAction();

        public abstract int Count { get; }
        
        protected override void OnCreateIndex(TK key, IReactiveProperty<T> value)
        {
            base.OnCreateIndex(key, value);
            onAdd.Invoke(key, value);
        }
        
        public void Add(TA item)
        {
            var (key, value) = AddToCollection(item);
            onAdd.Invoke(key, value);
        }

        protected abstract (TK key, IReactiveProperty<T> value) AddToCollection(TA item);

        public bool Remove(TR item)
        {
            var (success, key, value) = RemoveFromCollection(item);
            if (success)
            {
                onRemove.Invoke(key, value);
            }
            return success;
        }
        
        protected abstract (bool, TK key, IReactiveProperty<T> value) RemoveFromCollection(TR item);

        public abstract bool Contains(T item);

        public void Clear()
        {
            ClearCollection();
            onClear.Invoke();
        }

        protected abstract void ClearCollection();
        
        public TI SafeBindOnAddAction(IDestroyable destroyable, Action<TK, IReactiveProperty<T>> action)
        {
            onAdd.SafeBind(destroyable, action);
            return Builder;
        }

        public TI UnbindOnAddAction(Action<TK, IReactiveProperty<T>> action)
        {
            onAdd.Unbind(action);
            return Builder;
        }

        public TI SafeBindOnRemoveAction(IDestroyable destroyable, Action<TK, IReactiveProperty<T>> action)
        {
            onRemove.SafeBind(destroyable, action);
            return Builder;
        }

        public TI UnbindOnRemoveAction(Action<TK, IReactiveProperty<T>> action)
        {
            onRemove.Unbind(action);
            return Builder;
        }

        public override void Release()
        {
            base.Release();
            onAdd.Release();
            onRemove.Release();
            onClear.Release();
        }
    }
}