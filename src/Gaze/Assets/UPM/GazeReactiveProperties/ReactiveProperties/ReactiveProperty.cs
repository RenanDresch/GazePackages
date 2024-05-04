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

		public readonly Binder<TValue> OnChange = new();
		public readonly Binder<TValue, TValue> OnReplace = new();
		
		void SetValue(
			TValue value
		)
		{
			DisposedCheck();
			var oldValue = Value;
			
			if (Comparer.IsEquals(Value, value))
			{
				return;
			}
			
			Value = value;
			
			OnChange.Invoke(Value);
			OnReplace.Invoke(Value, oldValue);
		}

		internal override void OnDispose()
		{
			OnChange.Dispose();
			OnReplace.Dispose();
		}
		
		public static implicit operator TValue(ReactiveProperty<TValue> reactiveProperty) => reactiveProperty.Value;
	}
}