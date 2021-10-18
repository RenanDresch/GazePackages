using System.Collections.Generic;
using Gaze.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.MVVM.Model
{
    [System.Serializable]
    public class ReactiveStack<T> : ReactiveProperty<Stack<T>>
    {
        Stack<T> internalStack;

        readonly UnityEvent<T,T> onPush = new UnityEvent<T, T>();
        readonly UnityEvent<T,T> onPop = new UnityEvent<T, T>();
        readonly UnityEvent onClear = new UnityEvent();
        
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
        
        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is pushed into the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets pushed into the stack.
        /// The first argument represents the new pushed item, the second one represents the former stack top</param>
        public void SafeBindOnPushAction(IDestroyable destroyable, UnityAction<T,T> action)
        {
            if (destroyable != null)
            {
                onPush.AddListener(action);
                destroyable.OnDestroyEvent.AddListener(() => onPush.RemoveListener(action));
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
        public void SafeBindOnPopAction(IDestroyable destroyable, UnityAction<T,T> action)
        {
            if (destroyable != null)
            {
                onPop.AddListener(action);
                destroyable.OnDestroyEvent.AddListener(() => onPop.RemoveListener(action));
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
        public void SafeBindOnClearAction(IDestroyable destroyable, UnityAction action)
        {
            if (destroyable != null)
            {
                onClear.AddListener(action);
                destroyable.OnDestroyEvent.AddListener(() => onClear.RemoveListener(action));
            }
            else
            {
                Debug.LogError("Cannot safely bind to a reactive property without a lifecycle observer");
            }
        }
    }
}
