namespace Neusta.Shared.Logging.Base
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Logging.Adapter;

	public abstract class BaseSimplifiedLogger : ILogger
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string name;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isTraceEnabled;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isDebugEnabled;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isInfoEnabled;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isWarnEnabled;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isErrorEnabled;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool isFatalEnabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSimplifiedLogger"/> class.
		/// </summary>
		protected BaseSimplifiedLogger(string name)
		{
			this.name = name;

			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateIsEnabledValues();
		}

		#region Implementation of ILogger

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.name; }
		}

		#region State

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Trace</c> level.
		/// </summary>
		public bool IsTraceEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isTraceEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Debug</c> level.
		/// </summary>
		public bool IsDebugEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isDebugEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Info</c> level.
		/// </summary>
		public bool IsInfoEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isInfoEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Warn</c> level.
		/// </summary>
		public bool IsWarnEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isWarnEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Error</c> level.
		/// </summary>
		public bool IsErrorEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isErrorEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the <c>Fatal</c> level.
		/// </summary>
		public bool IsFatalEnabled
		{
			[DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return this.isFatalEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		public abstract bool IsEnabled(LogLevel level);

		/// <summary>
		/// Updates the logger settings.
		/// </summary>
		public virtual void UpdateLoggerSettings()
		{
			this.UpdateIsEnabledValues();
		}

		#endregion

		#region LogEvent

		/// <summary>
		/// Creates an empty log event.
		/// </summary>
		public virtual ILogEvent CreateLogEvent(LogLevel level, string message, Exception exception, params object[] args)
		{
			return new LogEvent(this, level, message, exception, args);
		}

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(ILogEvent logEvent)
		{
			if (this.IsEnabled(logEvent.Level))
			{
				this.InternalLog(logEvent);
			}
		}

		#endregion

		#region Logging

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(LogLevel level, Func<string> builderFunc)
		{
			if (this.IsEnabled(level))
			{
				this.Log(level, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LogException(LogLevel level, string message, Exception exception)
		{
			if (this.IsEnabled(level))
			{
				this.InternalLog(level, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LogException(LogLevel level, Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.IsEnabled(level))
			{
				this.InternalLog(level, builderFunc(exception));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.IsEnabled(level))
			{
				this.InternalLog(level, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(LogLevel level, string message)
		{
			if (this.IsEnabled(level))
			{
				this.InternalLog(level, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(LogLevel level, string message, params object[] args)
		{
			if (this.IsEnabled(level))
			{
				this.InternalLog(level, string.Format(message, args));
			}
		}

		#endregion

		#region Trace

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(Func<string> builderFunc)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TraceException(string message, Exception exception)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Trace</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TraceException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(string message)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Trace</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(string message, params object[] args)
		{
			if (this.isTraceEnabled)
			{
				this.InternalLog(LogLevel.Trace, string.Format(message, args));
			}
		}

		#endregion

		#region Debug

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(Func<string> builderFunc)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugException(string message, Exception exception)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Debug</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(string message)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Debug</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(string message, params object[] args)
		{
			if (this.isDebugEnabled)
			{
				this.InternalLog(LogLevel.Debug, string.Format(message, args));
			}
		}

		#endregion

		#region Info

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(Func<string> builderFunc)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InfoException(string message, Exception exception)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Info</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InfoException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(string message)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Info</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(string message, params object[] args)
		{
			if (this.isInfoEnabled)
			{
				this.InternalLog(LogLevel.Info, string.Format(message, args));
			}
		}

		#endregion

		#region Warn

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warn(Func<string> builderFunc)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WarnException(string message, Exception exception)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Warn</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WarnException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warn(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warn(string message)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Warn</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warn(string message, params object[] args)
		{
			if (this.isWarnEnabled)
			{
				this.InternalLog(LogLevel.Warn, string.Format(message, args));
			}
		}

		#endregion

		#region Error

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(Func<string> builderFunc)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ErrorException(string message, Exception exception)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Error</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ErrorException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string message)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Error</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string message, params object[] args)
		{
			if (this.isErrorEnabled)
			{
				this.InternalLog(LogLevel.Error, string.Format(message, args));
			}
		}

		#endregion

		#region Fatal

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Fatal(Func<string> builderFunc)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, builderFunc());
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FatalException(string message, Exception exception)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the <c>Fatal</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FatalException(Func<Exception, string> builderFunc, Exception exception)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, builderFunc(exception), exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters and formatting them with the supplied format provider.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, string.Format(formatProvider, message, args));
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Fatal(string message)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the <c>Fatal</c> level using the specified parameters.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Fatal(string message, params object[] args)
		{
			if (this.isFatalEnabled)
			{
				this.InternalLog(LogLevel.Fatal, string.Format(message, args));
			}
		}

		#endregion

		#endregion

		#region Protected Methods

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void InternalLog(ILogEvent logEvent)
		{
			string formattedMessage = logEvent.GetFormattedMessage();
			if (logEvent.Exception != null)
			{
				this.InternalLog(logEvent.Level, formattedMessage, logEvent.Exception);
			}
			else
			{
				this.InternalLog(logEvent.Level, formattedMessage);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract void InternalLog(LogLevel level, string message);

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract void InternalLog(LogLevel level, string message, Exception exception);

		/// <summary>
		/// Updates the IsEnabled values.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void UpdateIsEnabledValues()
		{
			this.isTraceEnabled = this.IsEnabled(LogLevel.Trace);
			this.isDebugEnabled = this.IsEnabled(LogLevel.Debug);
			this.isInfoEnabled = this.IsEnabled(LogLevel.Info);
			this.isWarnEnabled = this.IsEnabled(LogLevel.Warn);
			this.isErrorEnabled = this.IsEnabled(LogLevel.Error);
			this.isFatalEnabled = this.IsEnabled(LogLevel.Fatal);
		}

		#endregion
	}
}