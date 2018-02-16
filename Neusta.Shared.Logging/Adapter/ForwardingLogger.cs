namespace Neusta.Shared.Logging.Adapter
{
	using System;
	using System.Diagnostics;

	public class ForwardingLogger : IForwardingLogger
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ILogger targetLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ForwardingLogger"/> class.
		/// </summary>
		public ForwardingLogger(ILogger targetLogger)
		{
			while (targetLogger is IForwardingLogger forwardingLogger)
			{
				targetLogger = forwardingLogger.TargetLogger;
			}
			this.targetLogger = targetLogger;
		}

		#region Implementation of IForwardingLogger

		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		public ILogger TargetLogger
		{
			[DebuggerStepThrough]
			get { return this.targetLogger; }
			[DebuggerStepThrough]
			set
			{
				while (value is IForwardingLogger forwardingLogger)
				{
					value = forwardingLogger.TargetLogger;
				}
				this.targetLogger = value;
			}
		}

		#endregion

		#region Implementation of ILogger

		public string Name => this.targetLogger.Name;

		#region State

		public bool IsTraceEnabled => this.targetLogger.IsTraceEnabled;
		public bool IsDebugEnabled => this.targetLogger.IsDebugEnabled;
		public bool IsInfoEnabled => this.targetLogger.IsInfoEnabled;
		public bool IsWarnEnabled => this.targetLogger.IsWarnEnabled;
		public bool IsErrorEnabled => this.targetLogger.IsErrorEnabled;
		public bool IsFatalEnabled => this.targetLogger.IsFatalEnabled;

		public virtual void UpdateLoggerSettings()
		{
			this.targetLogger.UpdateLoggerSettings();
		}

		public bool IsEnabled(LogLevel level)
		{
			return this.targetLogger.IsEnabled(level);
		}

		#endregion

		#region LogEvent

		public ILogEvent CreateLogEvent(LogLevel level, string message, Exception exception, params object[] args)
		{
			return this.targetLogger.CreateLogEvent(level, message, exception, args);
		}

		#endregion

		#region Logging

		public void Log(ILogEvent logEvent)
		{
			this.targetLogger.Log(logEvent);
		}

		public void Log(LogLevel level, Func<string> builderFunc)
		{
			this.targetLogger.Log(level, builderFunc);
		}

		public void LogException(LogLevel level, string message, Exception exception)
		{
			this.targetLogger.LogException(level, message, exception);
		}

		public void LogException(LogLevel level, Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.LogException(level, builderFunc, exception);
		}

		public void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Log(level, formatProvider, message, args);
		}

		public void Log(LogLevel level, string message)
		{
			this.targetLogger.Log(level, message);
		}

		public void Log(LogLevel level, string message, params object[] args)
		{
			this.targetLogger.Log(level, message, args);
		}

		#endregion

		#region Trace

		public void Trace(Func<string> builderFunc)
		{
			this.targetLogger.Trace(builderFunc);
		}

		public void TraceException(string message, Exception exception)
		{
			this.targetLogger.TraceException(message, exception);
		}

		public void TraceException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.TraceException(builderFunc, exception);
		}

		public void Trace(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Trace(formatProvider, message, args);
		}

		public void Trace(string message)
		{
			this.targetLogger.Trace(message);
		}

		public void Trace(string message, params object[] args)
		{
			this.targetLogger.Trace(message, args);
		}

		#endregion

		#region Debug

		public void Debug(Func<string> builderFunc)
		{
			this.targetLogger.Debug(builderFunc);
		}

		public void DebugException(string message, Exception exception)
		{
			this.targetLogger.DebugException(message, exception);
		}

		public void DebugException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.DebugException(builderFunc, exception);
		}

		public void Debug(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Debug(formatProvider, message, args);
		}

		public void Debug(string message)
		{
			this.targetLogger.Debug(message);
		}

		public void Debug(string message, params object[] args)
		{
			this.targetLogger.Debug(message, args);
		}

		#endregion

		#region Info

		public void Info(Func<string> builderFunc)
		{
			this.targetLogger.Info(builderFunc);
		}

		public void InfoException(string message, Exception exception)
		{
			this.targetLogger.InfoException(message, exception);
		}

		public void InfoException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.InfoException(builderFunc, exception);
		}

		public void Info(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Info(formatProvider, message, args);
		}

		public void Info(string message)
		{
			this.targetLogger.Info(message);
		}

		public void Info(string message, params object[] args)
		{
			this.targetLogger.Info(message, args);
		}

		#endregion

		#region Warn

		public void Warn(Func<string> builderFunc)
		{
			this.targetLogger.Warn(builderFunc);
		}

		public void WarnException(string message, Exception exception)
		{
			this.targetLogger.WarnException(message, exception);
		}

		public void WarnException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.WarnException(builderFunc, exception);
		}

		public void Warn(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Warn(formatProvider, message, args);
		}

		public void Warn(string message)
		{
			this.targetLogger.Warn(message);
		}

		public void Warn(string message, params object[] args)
		{
			this.targetLogger.Warn(message, args);
		}

		#endregion

		#region Error

		public void Error(Func<string> builderFunc)
		{
			this.targetLogger.Error(builderFunc);
		}

		public void ErrorException(string message, Exception exception)
		{
			this.targetLogger.InfoException(message, exception);
		}

		public void ErrorException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.ErrorException(builderFunc, exception);
		}

		public void Error(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Error(formatProvider, message, args);
		}

		public void Error(string message)
		{
			this.targetLogger.Error(message);
		}

		public void Error(string message, params object[] args)
		{
			this.targetLogger.Error(message, args);
		}

		#endregion

		#region Fatal

		public void Fatal(Func<string> builderFunc)
		{
			this.targetLogger.Fatal(builderFunc);
		}

		public void FatalException(string message, Exception exception)
		{
			this.targetLogger.FatalException(message, exception);
		}

		public void FatalException(Func<Exception, string> builderFunc, Exception exception)
		{
			this.targetLogger.FatalException(builderFunc, exception);
		}

		public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
		{
			this.targetLogger.Fatal(formatProvider, message, args);
		}

		public void Fatal(string message)
		{
			this.targetLogger.Fatal(message);
		}

		public void Fatal(string message, params object[] args)
		{
			this.targetLogger.Fatal(message, args);
		}

		#endregion

		#endregion
	}
}
