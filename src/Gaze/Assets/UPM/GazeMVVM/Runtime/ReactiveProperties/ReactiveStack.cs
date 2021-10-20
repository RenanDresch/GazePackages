using System;
using System.Collections.Generic;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public class ReactiveStack<T> : ReactiveProperty<Stack<T>>, IReactiveStack<T>
    {
        Stack<T> internalStack;

        Action<T,T> onPush;
        Action<T,T> onPop;
        Action onClear;
        
        public ReactiveStack(T topItem = default)
        {
            internalStack = currentValue = new Stack<T>();
            if (topItem != null)
            {
                internalStack.Push(topItem);
            }
        }

        public int Count => internalStack.Count;

        public T Peek() => internalStack.Peek();

        public T Pop()
        {
            var item = internalStack.Pop();
            OnPropertyChangeEvent?.Invoke(internalStack);
            if (Count < 1)
            {
                onClear?.Invoke();
            }
            else
            {
                onPop?.Invoke(item, Peek());
            }
            return item;
        }

        public void Clear()
        {
            internalStack.Clear();
            OnPropertyChangeEvent?.Invoke(internalStack);
            onClear?.Invoke();
        }
        
        public void Push(T item)
        {
            var formerStackTop = default(T);

            if (Count > 0)
            {
                formerStackTop = Peek();
            }
            
            internalStack.Push(item);
            OnPropertyChangeEvent?.Invoke(internalStack);
            onPush?.Invoke(item, formerStackTop);
        }
        
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is pushed into the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets pushed into the stack.
        /// The first argument represents the new pushed item, the second one represents the former stack top</param>
        public void SafeBindOnPushAction(IDestroyable destroyable, Action<T,T> action)
        {
            if (destroyable != null)
            {
                onPush += action;
                destroyable.OnDestroyEvent += () => onPush -= action;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }
        
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is popped from the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.
        /// The first argument represents the popped item, the second one represents the new stack top</param>
        public void SafeBindOnPopAction(IDestroyable destroyable, Action<T,T> action)
        {
            if (destroyable != null)
            {
                onPop += action;
                destroyable.OnDestroyEvent += () => onPop -= action;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }
        
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        public void SafeBindOnClearAction(IDestroyable destroyable, Action action)
        {
            if (destroyable != null)
            {
                onClear += action;
                destroyable.OnDestroyEvent += () => onClear -= action;
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }
    }
}
