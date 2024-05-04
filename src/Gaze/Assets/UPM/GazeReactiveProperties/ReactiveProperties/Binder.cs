using System;

namespace Gaze.ReactiveProperties
{
	public class Binder<T1>
	{
		Action<T1> callback = _ => {}; //Skip null check
		
		public IDisposable Bind(
			Action<T1> callback
		)
		{
			this.callback += callback;
			return new DisposalToken(() => this.callback -= callback);
		}
		
		public void Unbind(
			Action<T1> callback
		)
		{
			this.callback -= callback;
		}
		
		internal void Invoke(
			T1 value
		)
		{
			callback(value);
		}

		internal void Dispose()
		{
			callback = null;
		}
	}
	
	public class Binder<T1, T2>
	{
		Action<T1, T2> callback = (_, _) => {}; //Skip null check
		
		public IDisposable Bind(
			Action<T1, T2> callback
		)
		{
			this.callback += callback;
			return new DisposalToken(() => this.callback -= callback);
		}
		
		public void Unbind(
			Action<T1, T2> callback
		)
		{
			this.callback -= callback;
		}
		
		internal void Invoke(
			T1 value1,
			T2 value2
		)
		{
			callback(value1, value2);
		}

		internal void Dispose()
		{
			callback = null;
		}
	}
	
	public class Binder<T1, T2, T3>
	{
		Action<T1, T2, T3> callback = (_, _, _) => {}; //Skip null check
		
		public IDisposable Bind(
			Action<T1, T2, T3> callback
		)
		{
			this.callback += callback;
			return new DisposalToken(() => this.callback -= callback);
		}
		
		public void Unbind(
			Action<T1, T2, T3> callback
		)
		{
			this.callback -= callback;
		}
		
		internal void Invoke(
			T1 value1,
			T2 value2,
			T3 value3
		)
		{
			callback(value1, value2, value3);
		}

		internal void Dispose()
		{
			callback = null;
		}
	}
}