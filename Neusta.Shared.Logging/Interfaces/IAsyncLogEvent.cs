namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Utils;

	public interface IAsyncLogEvent
	{
		/// <summary>
		/// Gets the log event.
		/// </summary>
		[PublicAPI]
		ILogEvent LogEvent { get; }

		/// <summary>
		/// Gets the continuation.
		/// </summary>
		[PublicAPI]
		AsyncContinuation Continuation { get; }
	}
}