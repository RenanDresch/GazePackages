using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IChangeable<out TI>
    {
        TI SafeBindOnChangeAction(IDestroyable destroyable, Action<TI> action);
        TI UnbindOnChangeAction(Action<TI> action);
    }
}