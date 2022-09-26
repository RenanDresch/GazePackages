using System;

namespace Gaze.MVVM
{
    public interface ITriggerable<out T>
    {
        void Trigger(Action<T> action);
    }
    public interface ITriggerable<out T1, out T2>
    {
        void Trigger(Action<T1,T2> action);
    }
}