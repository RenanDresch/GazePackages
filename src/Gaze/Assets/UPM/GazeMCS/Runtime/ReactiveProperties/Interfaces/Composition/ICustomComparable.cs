using System;

namespace Gaze.MCS
{
    public interface ICustomComparable<out TI, out TP>
    {
        TI WithCustomComparer(Func<TP, TP, bool> customComparer);
    }
}