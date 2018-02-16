namespace Neusta.Shared.Logging.Adapter.Null
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Logging.Configuration.Filter;

	internal sealed class NullLogger : ILogger
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NullLogger"/> class.
		/// </summary>
		public NullLogger()
		{
		}

		#region Implementation of ILogger

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return "NullLogger"; }
		}

		#region Filter

		/// <summary>
		/// Gets the filter.
		/// </summary>
		public Func<string, string, LogLevel, bool> Filter
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return FilterRule.False; }
		}

		#endregion

		#region States

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Trace</c> level.
		/// </summary>
		public bool IsTraceEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Debug</c> level.
		/// </summary>
		public bool IsDebugEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Info</c> level.
		/// </summary>
		public bool IsInfoEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Warn</c> level.
		/// </summary>
		public bool IsWarnEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Error</c> level.
		/// </summary>
		public bool IsErrorEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Fatal</c> level.
		/// </summary>
		public bool IsFatalEnabled
		{
			[DebuggerStepThrough]
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		public bool IsEnabled(LogLevel level)
		{
			return false;
		}

		/// <summary>
		/// Updates the logger settings.
		/// </summary>
		public void UpdateLoggerSettings()
		{
		}

		#endregion

		#region LogEvent

		public ILogEvent CreateLogEvent(LogLevel level, string message, Exception exception, params object[] args)
		{
			return new LogEvent(this, level, message, exception, args);
		}

		#endregion

		#region Logging

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		public void Log(ILogEvent logEvent)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		public void Log(LogLevel level, Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		public void LogException(LogLevel level, string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		public void LogException(LogLevel level, Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		public void Log(LogLevel level, string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters.
		/// </summary>
		public void Log(LogLevel level, string message, params object[] args)
		{
		}

		#endregion

		#region Trace

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		public void Trace(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		public void TraceException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		public void TraceException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Trace(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		public void Trace(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters.
		/// </summary>
		public void Trace(string message, params object[] args)
		{
		}

		#endregion

		#region Debug

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		public void Debug(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		public void DebugException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		public void DebugException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Debug(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		public void Debug(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
		/// </summary>
		public void Debug(string message, params object[] args)
		{
		}

		#endregion

		#region Info

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		public void Info(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		public void InfoException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		public void InfoException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Info(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		public void Info(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters.
		/// </summary>
		public void Info(string message, params object[] args)
		{
		}

		#endregion

		#region Warn

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		public void Warn(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		public void WarnException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		/// </summary>
		public void WarnException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Warn(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		public void Warn(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters.
		/// </summary>
		public void Warn(string message, params object[] args)
		{
		}

		#endregion

		#region Error

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		public void Error(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		public void ErrorException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		public void ErrorException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Error(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		public void Error(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters.
		/// </summary>
		public void Error(string message, params object[] args)
		{
		}

		#endregion

		#region Fatal

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		public void Fatal(Func<string> builderFunc)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		public void FatalException(string message, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		public void FatalException(Func<Exception, string> builderFunc, Exception exception)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		public void Fatal(string message)
		{
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters.
		/// </summary>
		public void Fatal(string message, params object[] args)
		{
		}

		#endregion

		#endregion
	}
}