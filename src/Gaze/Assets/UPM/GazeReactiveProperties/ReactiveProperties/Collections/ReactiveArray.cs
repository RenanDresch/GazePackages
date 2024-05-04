using System;

namespace Gaze.ReactiveProperties.Collections
{
	[Serializable]
	public class ReadOnlyReactiveArray<TValue> : Comparable<TValue>
	{
		protected TValue[] InternalArray;
		public readonly Binder<int, TValue> OnIndexSet = new();
		public readonly Binder<int, TValue, TValue> OnIndexReplace = new();
		
		public TValue this[
			int index
		]
		{
			get => InternalArray[index];
			set => SetValue(index, value);
		}
		
		public int Length => InternalArray.Length;
		
		protected ReadOnlyReactiveArray(
			int length,
			TValue defaultValue = default
		)
		{
			InternalArray = new TValue[length];
			for (var i = 0; i < length; i++)
			{
				InternalArray[i] = defaultValue;
			}
		}
		
		void SetValue(
			int index,
			TValue value
		)
		{
			DisposedCheck();
			var oldValue = InternalArray[index];
			
			if (Comparer.IsEquals(oldValue, value))
			{
				return;
			}
			
			InternalArray[index] = value;
			
			OnIndexReplace.Invoke(index, value, oldValue);
			OnIndexSet.Invoke(index, value);
		}
		
		internal override void OnDispose()
		{
			OnIndexSet.Dispose();
			OnIndexReplace.Dispose();
		}
		
		internal void DoubleCapacity()
		{
			if(InternalArray.Length == 0)
			{
				InternalArray = new TValue[1];
				return;
			}
			
			var newInternalArray = new TValue[InternalArray.Length * 2];
			for (var i = 0; i < InternalArray.Length; i++)
			{
				newInternalArray[i] = InternalArray[i];
			}
			InternalArray = newInternalArray;
		}
	}
}