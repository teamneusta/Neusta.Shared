namespace Neusta.Shared.Logging.Utils
{
	using System;
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class AnonymousDisposable : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AnonymousDisposable"/> class.
		/// </summary>
		public AnonymousDisposable()
		{
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		#endregion
	}
}