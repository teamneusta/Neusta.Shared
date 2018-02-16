namespace Neusta.Shared.Logging.Adapter.Helper
{
	using System;
	using System.Collections;
	using JetBrains.Annotations;

	internal sealed class LogEventSyntaxHelper : IFluentSyntax,
		ILogSyntax, ILogSyntaxResult
	{
		private readonly ILogger logger;
		private readonly ILogEvent logEvent;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogEventSyntaxHelper"/> class.
		/// </summary>
		public LogEventSyntaxHelper(ILogger logger)
			: this(logger, LogLevel.Debug)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogEventSyntaxHelper"/> class.
		/// </summary>
		public LogEventSyntaxHelper(ILogger logger, LogLevel logLevel)
		{
			this.logger = logger;
			this.logEvent = logger.CreateLogEvent(logLevel, null, null);
		}

		/// <summary>
		/// Gets the <see cref="ILogEvent"/> created by the builder.
		/// </summary>
		[PublicAPI]
		public ILogEvent LogEvent => this.logEvent;

		#region Implementation of ILogSyntax

		/// <summary>
		/// Sets the logger name of the logging event.
		/// </summary>
		public ILogSyntaxResult LoggerName(string loggerName)
		{
			this.logEvent.LoggerName = loggerName;
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message)
		{
			this.logEvent.Message = message;
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message, object arg0)
		{
			this.logEvent.Message = message;
			this.logEvent.Parameters = new object[] { arg0 };
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message, object arg0, object arg1)
		{
			this.logEvent.Message = message;
			this.logEvent.Parameters = new object[] { arg0, arg1 };
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message, object arg0, object arg1, object arg2)
		{
			this.logEvent.Message = message;
			this.logEvent.Parameters = new object[] { arg0, arg1, arg2 };
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message, object arg0, object arg1, object arg2, object arg3)
		{
			this.logEvent.Message = message;
			this.logEvent.Parameters = new object[] { arg0, arg1, arg2, arg3 };
			return this;
		}

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		public ILogSyntaxResult Message(string message, params object[] args)
		{
			this.logEvent.Message = message;
			this.logEvent.Parameters = args;
			return this;
		}

		/// <summary>
		/// Sets the <paramref name="exception"/> information of the logging event.
		/// </summary>
		public ILogSyntaxResult Exception(Exception exception)
		{
			this.logEvent.Exception = exception;
			return this;
		}

		/// <summary>
		/// Sets the format provider.
		/// </summary>
		public ILogSyntaxResult FormatProvider(IFormatProvider formatProvider)
		{
			this.logEvent.FormatProvider = formatProvider;
			return this;
		}

		/// <summary>
		/// Sets a per-event context property on the logging event.
		/// </summary>
		public ILogSyntaxResult Property(object name, object value)
		{
			this.logEvent.Properties[name] = value;
			return this;
		}

		/// <summary>
		/// Sets multiple per-event context properties on the logging event.
		/// </summary>
		public ILogSyntaxResult Properties(IDictionary properties)
		{
			foreach (var key in properties.Keys)
			{
				this.logEvent.Properties[key] = properties[key];
			}
			return this;
		}

		#endregion

		#region Implementation of ILogSyntaxResult

		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		public void Write()
		{
			this.logger.Log(this.logEvent);
		}

		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		public void WriteIf(bool condition)
		{
			if (condition && this.logger.IsEnabled(this.logEvent.Level))
			{
				this.logger.Log(this.logEvent);
			}
		}

		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		public void WriteIf(Func<bool> condition)
		{
			if (((condition == null) || condition()) && this.logger.IsEnabled(this.logEvent.Level))
			{
				this.logger.Log(this.logEvent);
			}
		}

		#endregion
	}
}