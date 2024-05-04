using System;

namespace Gaze.ReactiveProperties
{
	public abstract class Disposable : IDisposable
	{
		public bool Disposed { get; private set; }
		
		internal void DisposedCheck()
		{
			if (Disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public void Dispose()
		{
			DisposedCheck();
			Disposed = true;
			OnDispose();
		}

		internal abstract void OnDispose();
	}
}