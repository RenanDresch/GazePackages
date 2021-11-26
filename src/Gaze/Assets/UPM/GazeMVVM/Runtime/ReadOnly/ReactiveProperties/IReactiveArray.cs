using System;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MVVM.ReadOnly
{
    public interface IReactiveArray<T> : IReactiveProperty<T[]>
    {
        int Lenght { get; }
        T this[int index] { get; }
        void SafeBindOnModifyItemAction(IDestroyable destroyable, Action<(int index, T newItem, T formerItem)> action);
    }
}
