using System;
using Gaze.Utilities;

namespace Gaze.MCS
{
    public interface IClearable<out TI>
    {
        void Clear();
        
        /// <summary>
        /// Binds an action to this Reactive collection so it's invoked whenever it gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets removed from the collection.</param>
        /// <returns>The Collection itself</returns>
        TI SafeBindOnClearAction(IDestroyable destroyable, Action action);
    }
}