using System;

namespace Gaze.ReactiveProperties
{
	public abstract class Comparable<TValue> : Disposable
	{
		internal readonly Comparer<TValue> Comparer = new();
		
		public void WithCustomComparer(Func<TValue, TValue, bool> customComparer)
		{
			DisposedCheck();
			Comparer.IsEquals = customComparer;
		}
	}
}