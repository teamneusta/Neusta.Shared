namespace Neusta.Shared.AspNetCore.Extensions.Logging
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Microsoft.Extensions.Logging;
	using Neusta.Shared.Logging;
	using LogLevel = Microsoft.Extensions.Logging.LogLevel;

	public class Logger : Microsoft.Extensions.Logging.ILogger
	{
		private readonly Neusta.Shared.Logging.ILogger logger;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LoggerOptions options;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IExternalScopeProvider scopeProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/> class.
		/// </summary>
		public Logger(Neusta.Shared.Logging.ILogger logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Gets or sets the logger options.
		/// </summary>
		internal LoggerOptions Options
		{
			[DebuggerStepThrough]
			get { return this.options; }
			[DebuggerStepThrough]
			set { this.options = value; }
		}

		/// <summary>
		/// Gets or sets the scope provider.
		/// </summary>
		internal IExternalScopeProvider ScopeProvider
		{
			[DebuggerStepThrough]
			get { return this.scopeProvider; }
			[DebuggerStepThrough]
			set { this.scopeProvider = value; }
		}

		#region Implementation of ILogger

		/// <summary>
		/// Writes a log entry.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			// Filter
			if (!this.IsEnabled(logLevel))
			{
				return;
			}

			// Format message
			string message = formatter(state, exception);
			var translatedLogLevel = LogLevelTranslator.Translate(logLevel);

			// Check for scope
			var innerScopeProvider = this.scopeProvider;
			if (this.options.IncludeScopes && (innerScopeProvider != null))
			{
				ILogEvent logEvent = this.logger.CreateLogEvent(translatedLogLevel, message, exception);

				// Add scope details
				innerScopeProvider.ForEachScope(delegate (object _scope, ILogEvent _logEvent)
				{
					if (_scope is IEnumerable<KeyValuePair<string, object>> _logValues)
					{
						var _properties = _logEvent.Properties;
						foreach (var _logValue in _logValues)
						{
							_properties[_logValue.Key] = _logValue.Value;
						}
					}
					else
					{
						_logEvent.Message = _logEvent.Message + " => " + _scope.ToString();
					}
				}, logEvent);

				// Process the log event
				this.logger.Log(logEvent);
			}
			else if (exception != null)
			{
				// Unscoped exception logging
				this.logger.LogException(translatedLogLevel, message, exception);
			}
			else
			{
				// Unscoped message logging
				this.logger.Log(translatedLogLevel, message);
			}
		}

		/// <summary>
		/// Checks if the given <paramref name="logLevel" /> is enabled.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(LogLevel logLevel)
		{
			if (logLevel == LogLevel.None)
			{
				return false;
			}
			return this.logger.IsEnabled(LogLevelTranslator.Translate(logLevel));
		}

		/// <summary>
		/// Begins a logical operation scope.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IDisposable BeginScope<TState>(TState state)
		{
			return this.scopeProvider?.Push(state) ?? NullScope.Instance;
		}

		#endregion
	}
}