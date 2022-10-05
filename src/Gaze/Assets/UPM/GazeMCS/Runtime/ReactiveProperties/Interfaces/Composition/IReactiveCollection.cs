using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IReactiveCollection<out TI, TK, T, in TA, in TR> :
        IReactiveIndexable<TI, TK, T>
    {
        int Count { get; }
        void Add(TA item);
        bool Remove(TR item);
        bool Contains(T item);
        void Clear();
        
        
        TI SafeBindOnAddAction(IDestroyable destroyable, Action<TK, IReactiveProperty<T>> action);
        TI UnbindOnAddAction(Action<TK, IReactiveProperty<T>> action);
        TI SafeBindOnRemoveAction(IDestroyable destroyable, Action<TK, IReactiveProperty<T>> action);
        TI UnbindOnRemoveAction(Action<TK, IReactiveProperty<T>> action);
    }
}