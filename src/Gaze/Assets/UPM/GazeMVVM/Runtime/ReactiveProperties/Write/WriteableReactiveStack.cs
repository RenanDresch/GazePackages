using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;

namespace Gaze.MVVM
{
    [Serializable]
    public class WriteableReactiveStack<T> : WriteableReactiveProperty<IEnumerable<T>>, IReactiveStack<T>
    {
        Stack<T> internalStack;

        SafeAction<T,T> onPush = new SafeAction<T, T>();
        SafeAction<T,T> onPop = new SafeAction<T, T>();
        SafeAction onClear = new SafeAction();
        
        public WriteableReactiveStack(T topItem = default)
        {
            internalStack = new Stack<T>();
            if (topItem != null)
            {
                internalStack.Push(topItem);
            }
        }

        public override IEnumerable<T> Value
        {
            get => internalStack;
            set
            {
                internalStack = new Stack<T>(value);
                OnPropertyChangeEvent.Invoke(internalStack);
            }
        }

        public int Count => internalStack.Count;

        public T Peek() => internalStack.Peek();

        public void Push(T item)
        {
            var formerStackTop = default(T);

            if (Count > 0)
            {
                formerStackTop = Peek();
            }
            
            internalStack.Push(item);
            OnPropertyChangeEvent.Invoke(internalStack);
            onPush.Invoke(item, formerStackTop);
        }
        
        public T Pop()
        {
            var item = internalStack.Pop();
            OnPropertyChangeEvent.Invoke(internalStack);
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
            internalStack.Clear();
            OnPropertyChangeEvent.Invoke(internalStack);
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
    }
}
