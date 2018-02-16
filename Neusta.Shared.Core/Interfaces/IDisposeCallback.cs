namespace Neusta.Shared.Core
{
	using System;

	public interface IDisposeCallback
	{
		/// <summary>
		/// This event should be triggered when implementing instance is disposed.
		/// </summary>
		event EventHandler OnDispose;
	}
}