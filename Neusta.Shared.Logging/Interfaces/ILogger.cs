namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;

	public interface ILogger
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		[PublicAPI]
		string Name { get; }

		#region State

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		bool IsTraceEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		bool IsDebugEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		bool IsInfoEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Warn</c> level.
		/// </summary>
		[PublicAPI]
		bool IsWarnEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		bool IsErrorEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		bool IsFatalEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		[PublicAPI]
		bool IsEnabled(LogLevel level);

		/// <summary>
		/// Updates the logger settings.
		/// </summary>
		[PublicAPI]
		void UpdateLoggerSettings();

		#endregion

		#region LogEvent

		/// <summary>
		/// Creates an empty log event.
		/// </summary>
		[PublicAPI]
		ILogEvent CreateLogEvent(LogLevel level, string message, Exception exception, params object[] args);

		#endregion

		#region Logging

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		[PublicAPI]
		void Log(ILogEvent logEvent);

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[PublicAPI]
		void Log(LogLevel level, Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[PublicAPI]
		void LogException(LogLevel level, string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[PublicAPI]
		void LogException(LogLevel level, Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[PublicAPI]
		void Log(LogLevel level, string message);

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Log(LogLevel level, string message, params object[] args);

		#endregion

		#region Trace

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		void Trace(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		void TraceException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		void TraceException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Trace(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		[PublicAPI]
		void Trace(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Trace(string message, params object[] args);

		#endregion

		#region Debug

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		void Debug(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		void DebugException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		void DebugException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Debug(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		[PublicAPI]
		void Debug(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Debug(string message, params object[] args);

		#endregion

		#region Info

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		void Info(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		void InfoException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		void InfoException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Info(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		[PublicAPI]
		void Info(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Info(string message, params object[] args);

		#endregion

		#region Warn

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		[PublicAPI]
		void Warn(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		[PublicAPI]
		void WarnException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		/// </summary>
		[PublicAPI]
		void WarnException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Warn(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		[PublicAPI]
		void Warn(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Warn(string message, params object[] args);

		#endregion

		#region Error

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		void Error(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		void ErrorException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		void ErrorException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Error(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		[PublicAPI]
		void Error(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Error(string message, params object[] args);

		#endregion

		#region Fatal

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		void Fatal(Func<string> builderFunc);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		void FatalException(string message, Exception exception);

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		void FatalException(Func<Exception, string> builderFunc, Exception exception);

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[PublicAPI]
		void Fatal(IFormatProvider formatProvider, string message, params object[] args);

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		[PublicAPI]
		void Fatal(string message);

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters.
		/// </summary>
		[PublicAPI]
		void Fatal(string message, params object[] args);

		#endregion
	}
}