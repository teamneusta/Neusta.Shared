namespace Neusta.Shared.Logging
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ILogLevelSyntax<TResult> : IFluentSyntax
		where TResult : IFluentSyntax
	{
		/// <summary>
		/// A log level describing that everything should be logged (ALL).
		/// </summary>
		[PublicAPI]
		TResult All();

		/// <summary>
		/// The TRACE log level.
		/// </summary>
		[PublicAPI]
		TResult Trace();

		/// <summary>
		/// The DEBUG log level.
		/// </summary>
		[PublicAPI]
		TResult Debug();

		/// <summary>
		/// The INFO log level.
		/// </summary>
		[PublicAPI]
		TResult Info();

		/// <summary>
		/// The WARN log level.
		/// </summary>
		[PublicAPI]
		TResult Warn();

		/// <summary>
		/// The ERROR log level.
		/// </summary>
		[PublicAPI]
		TResult Error();

		/// <summary>
		/// The FATAL log level.
		/// </summary>
		[PublicAPI]
		TResult Fatal();

		/// <summary>
		/// A log level describing that no logging should happen (OFF).
		/// </summary>
		[PublicAPI]
		TResult Off();

		/// <summary>
		/// The given log level.
		/// </summary>
		[PublicAPI]
		TResult Set(LogLevel level);
	}
}