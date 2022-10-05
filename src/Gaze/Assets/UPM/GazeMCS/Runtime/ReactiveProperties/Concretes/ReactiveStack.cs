using System;
using System.Collections;
using System.Collections.Generic;
using Gaze.Utilities;

namespace Gaze.MCS
{
    [Serializable]
    public class ReactiveStack<T> : IReactiveStack<T>
    {
        readonly Stack<IReactiveProperty<T>> internalStack;

        readonly SafeAction<IReactiveStack<T>> onChange = new SafeAction<IReactiveStack<T>>();
        readonly SafeAction<IReactiveProperty<T>> onPush = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction<IReactiveProperty<T>> onPop = new SafeAction<IReactiveProperty<T>>();
        readonly SafeAction onClear = new SafeAction();

        /// <summary>
        /// Count the internal Stack 
        /// </summary>
        public int Count => internalStack.Count;

        /// <summary>
        /// Peeks the internal Stack 
        /// </summary>
        public IReactiveProperty<T> Peek => internalStack.Peek();
        
        /// <summary>
        /// Instantiates a ReactiveStack with the required capacity
        /// </summary>
        /// <param name="capacity">Stack capacity</param>
        public ReactiveStack(int capacity) => internalStack = new Stack<IReactiveProperty<T>>(capacity);
        
        /// <summary>
        /// Instantiates a ReactiveStack passing an existing collection
        /// </summary>
        /// <param name="stack">Collection to be used internally</param>
        public ReactiveStack(Stack<IReactiveProperty<T>> stack) => internalStack = stack;
        
        /// <summary>
        /// Pushes item to the stack top, triggering both OnChange and OnPush bindings, on that order
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The ReactiveStack itself</returns>
        public IReactiveStack<T> Push(IReactiveProperty<T> item)
        {
            internalStack.Push(item);
            onChange.Invoke(this);
            onPush.Invoke(item);
            return this;
        }
        
        /// <summary>
        /// Pops item from the stack top, triggering OnPop, OnChange and OnClear if stack becomes empty, on that order
        /// </summary>
        /// <returns>The popped item</returns>
        public IReactiveProperty<T> Pop()
        {
            var item = internalStack.Pop();
            onChange.Invoke(this);
            onPop.Invoke(item);
            if (internalStack.Count < 1)
            {
                onClear.Invoke();   
            }
            return item;
        }

        /// <summary>
        /// Clears the internal stack, triggering OnChange and OnClear, on that order
        /// </summary>
        public void Clear()
        {
            internalStack.Clear();
            onChange.Invoke(this);
            onClear.Invoke();
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever it's modified.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when the Stack changes.</param>
        /// <returns>The ReactiveStack itself</returns>
        public IReactiveStack<T> SafeBindOnChangeAction(IDestroyable destroyable, Action<IReactiveStack<T>> action)
        {
            onChange.SafeBind(destroyable, action);
            return this;
        }

        public IReactiveStack<T> UnbindOnChangeAction(Action<IReactiveStack<T>> action)
        {
            onChange.Unbind(action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is pushed into the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when a new item gets pushed into the stack.</param>
        /// <returns>The ReactiveStack itself</returns>
        public IReactiveStack<T> SafeBindOnPushAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onPush.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever a new item is popped from the stack.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute when an item gets popped from the stack.</param>
        /// <returns>The ReactiveStack itself</returns>
        public IReactiveStack<T> SafeBindOnPopAction(IDestroyable destroyable, Action<IReactiveProperty<T>> action)
        {
            onPop.SafeBind(destroyable, action);
            return this;
        }

        /// <summary>
        /// Binds an action to this Reactive Stack so it's invoked whenever the stack gets empty.
        /// If the IDestroyable is correctly setup, this binding avoids memory leaks.
        /// </summary>
        /// <param name="destroyable">The destroyable object that owns the target action.</param>
        /// <param name="action">The action to execute after the last item gets popped from the stack.</param>
        /// <returns>The ReactiveStack itself</returns>
        public IReactiveStack<T> SafeBindOnClearAction(IDestroyable destroyable, Action action)
        {
            onClear.SafeBind(destroyable, action);
            return this;
        }
        
        /// <summary>
        /// Unbinds all Actions from this Reactive Collection, allowing it to get collected. 
        /// </summary>
        public void Release()
        {
            onChange.Release();
            onPush.Release();
            onPop.Release();
            onClear.Release();
        }
        
        public IEnumerator<IReactiveProperty<T>> GetEnumerator() => internalStack.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => internalStack.GetEnumerator();
    }
}
