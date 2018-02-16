namespace Neusta.Shared.Logging.Utils
{
	using System;

	/// <summary>
	/// Asynchronous continuation delegate - function invoked at the end of asynchronous
	/// processing.
	/// </summary>
	public delegate void AsyncContinuation(Exception exception);
}