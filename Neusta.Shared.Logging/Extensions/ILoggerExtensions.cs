// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Adapter.Helper;

	// ReSharper disable once InconsistentNaming
	public static class ILoggerExtensions
	{
		/// <summary>
		/// Starts building a log event at the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Trace(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Trace);

		/// <summary>
		/// Starts building a log event at the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Debug(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Debug);

		/// <summary>
		/// Starts building a log event at the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Info(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Info);

		/// <summary>
		/// Starts building a log event at the <c>Warn</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Warn(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Warn);

		/// <summary>
		/// Starts building a log event at the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Error(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Error);

		/// <summary>
		/// Starts building a log event at the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Fatal(this ILogger logger) => new LogEventSyntaxHelper(logger, LogLevel.Fatal);

		/// <summary>
		/// Starts building a log event at the specified level.
		/// </summary>
		[PublicAPI]
		public static ILogSyntax Log(this ILogger logger, LogLevel logLevel) => new LogEventSyntaxHelper(logger, logLevel);
	}
}