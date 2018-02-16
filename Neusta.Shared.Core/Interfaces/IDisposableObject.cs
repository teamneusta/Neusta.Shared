namespace Neusta.Shared.Core
{
	using System;

	/// <summary>
	/// Extends the <see cref="IDisposable"/> interface with several information properties.
	/// </summary>
	public interface IDisposableObject : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		bool IsDisposed { get; }
	}
}