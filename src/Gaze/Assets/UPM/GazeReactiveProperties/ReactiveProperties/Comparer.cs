using System;
using System.Collections.Generic;

namespace Gaze.ReactiveProperties
{
	public class Comparer<TValue>
	{
		public Func<TValue, TValue, bool> IsEquals { get; set; } = EqualityComparer<TValue>.Default.Equals;
	}
}