using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveStack<T> : ReactiveProperty<Stack<T>>, IReactiveStack<T>
    {
        readonly SafeAction<T,T> onPush = new SafeAction<T, T>();
        readonly SafeAction<T,T> onPop = new SafeAction<T, T>();
        readonly SafeAction onClear = new SafeAction();
        
        public ReactiveStack(int capacity, T topItem = default)
        {
            Value = new Stack<T>(capacity);
            if (topItem != null)
            {
                Value.Push(topItem);
            }
        }

        public int Count => Value.Count;

        public T Peek() => Value.Peek();

        public void Push(T item)
        {
            var formerStackTop = default(T);

            if (Count > 0)
            {
                formerStackTop = Peek();
            }
            
            Value.Push(item);
            OnPropertyChangeEvent.Invoke(Value);
            onPush.Invoke(item, formerStackTop);
        }
        
        public T Pop()
        {
            var item = Value.Pop();
            OnPropertyChangeEvent.Invoke(Value);
            if (Count < 1)
            {
                onClear.Invoke();
            }
            else
            {
                onPop.Invoke(item, Peek());
            }
            return item;
        }

        public void Clear()
        {
            Value.Clear();
            OnPropertyChangeEvent.Invoke(Value);
            onClear.Invoke();
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is pushed into the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets pushed into the stack.
        /// The first argument represents the new pushed item, the second one represents the former stack top</param>
        public void SafeBindOnPushAction(IDestroyable destroyable, Action<T, T> action) => onPush.SafeBind(destroyable, action);

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is popped from the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.
        /// The first argument represents the popped item, the second one represents the new stack top</param>
        public void SafeBindOnPopAction(IDestroyable destroyable, Action<T, T> action) => onPop.SafeBind(destroyable, action);

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action) => onClear.SafeBind(destroyable, action);
        
        /// <summary>
        /// Unbinds all Actions from this Reactive Property, allowing it to get collected. 
        /// </summary>
        public void UnbindAllActions()
        {
            OnPropertyChangeEvent.UnbindAll();
            onPush.UnbindAll();
            onPop.UnbindAll();
            onClear.UnbindAll();
        }
    }
}
