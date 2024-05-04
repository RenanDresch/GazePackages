using System;

namespace Gaze.ReactiveProperties
{
	public class DisposalToken : IDisposable
	{
		readonly Action onDispose;
		
		public DisposalToken(Action onDispose)
		{
			this.onDispose = onDispose;
		}
		
		public void Dispose()
		{
			onDispose();
		}
	}
}