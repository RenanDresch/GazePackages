using System;

namespace Gaze.ReactiveProperties
{
	[Serializable]
	public class ReactiveProperty<TValue> : Comparable<TValue>
	{
		TValue internalValue;
		public TValue Value
		{
			get => internalValue;
			set => SetValue(value);
		}

		public ReactivePropertyBinder<TValue> Binder = new();
		
		void SetValue(
			TValue value
		)
		{
			DisposedCheck();
			var oldValue = internalValue;
			
			if (Comparer.IsEquals(internalValue, value))
			{
				return;
			}
			
			internalValue = value;
			
			Binder.OnChange.Invoke(internalValue);
			Binder.OnReplace.Invoke(internalValue, oldValue);
		}

		internal override void OnDispose()
		{
			Binder.Dispose();
		}
		
		public static implicit operator TValue(ReactiveProperty<TValue> reactiveProperty) => reactiveProperty.internalValue;
	}

	public class ReactivePropertyBinder<TValue> : IDisposable
	{
		public readonly Binder<TValue> OnChange = new();
		public readonly Binder<TValue, TValue> OnReplace = new();
		
		public void Dispose()
		{
			OnChange.Dispose();
			OnReplace.Dispose();
		}
	}
}