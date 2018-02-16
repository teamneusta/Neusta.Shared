// ReSharper disable once CheckNamespace
namespace System.Threading
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CancellationTokenSourceExtensions
	{
		/// <summary>
		/// Throws an <see cref="OperationCanceledException"/> if cancellation was requested.
		/// </summary>
		[PublicAPI]
		public static void ThrowIfCancellationRequested(this CancellationTokenSource cts)
		{
			cts.Token.ThrowIfCancellationRequested();
		}
	}
}