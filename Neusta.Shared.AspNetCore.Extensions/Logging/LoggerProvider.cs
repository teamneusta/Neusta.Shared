namespace Neusta.Shared.AspNetCore.Extensions.Logging
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Logging.Configuration.Filter;
	using ILogger = Microsoft.Extensions.Logging.ILogger;
	using LogLevel = Neusta.Shared.Logging.LogLevel;

	internal sealed class LoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
	{
		private readonly ConcurrentDictionary<string, Logger> loggerMap = new ConcurrentDictionary<string, Logger>();
		private readonly IOptionsMonitor<LoggerOptions> loggerOptions;
		private readonly IOptionsMonitor<LoggerFilterOptions> filterOptions;
		private readonly IDictionary<LoggerFilterRule, IFilterRule> filterRuleMap = new Dictionary<LoggerFilterRule, IFilterRule>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ILoggingAdapter loggingAdapter;

		private IDisposable optionsReloadToken;
		private IDisposable filterReloadToken;
		private IExternalScopeProvider scopeProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggerProvider"/> class.
		/// </summary>
		public LoggerProvider(ILoggingAdapter loggingAdapter, IOptionsMonitor<LoggerOptions> loggerOptions, IOptionsMonitor<LoggerFilterOptions> filterOptions)
		{
			this.loggingAdapter = loggingAdapter;
			this.loggerOptions = loggerOptions;
			this.filterOptions = filterOptions;

			this.ReloadLoggerOptions(loggerOptions.CurrentValue);
			this.ReloadFilterOptions(filterOptions.CurrentValue);
		}

		/// <summary>
		/// Gets the adapter.
		/// </summary>
		[PublicAPI]
		public ILoggingAdapter LoggingAdapter
		{
			[DebuggerStepThrough]
			get { return this.loggingAdapter; }
		}

		#region Implementation of ILoggerProvider

		/// <summary>
		/// Creates a new <see cref="ILogger" /> instance.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ILogger CreateLogger(string categoryName)
		{
			return this.loggerMap.GetOrAdd(categoryName, delegate(string innerName)
			{
				var logger = this.loggingAdapter.GetLogger(innerName);
				return new Logger(logger)
				{
					Options = this.loggerOptions.CurrentValue,
					ScopeProvider = this.scopeProvider
				};
			});
		}

		#endregion

		#region Implementation of ISupportExternalScope

		/// <summary>
		/// Sets external scope information source for logger provider.
		/// </summary>
		public void SetScopeProvider(IExternalScopeProvider scopeProvider)
		{
			this.scopeProvider = scopeProvider;
		}

		#endregion

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			this.optionsReloadToken?.Dispose();
			this.filterReloadToken?.Dispose();
			this.loggerMap.Clear();
		}

		#endregion

		#region Private Methods

		private void ReloadLoggerOptions(LoggerOptions options)
		{
			// Update known loggers
			foreach (var logger in this.loggerMap)
			{
				logger.Value.Options = options;
			}

			this.optionsReloadToken = this.loggerOptions.OnChange(this.ReloadLoggerOptions);
		}

		private void ReloadFilterOptions(LoggerFilterOptions options)
		{
			var configuration = this.loggingAdapter.Configuration;

			// Update minimum log level
			configuration.MinimumLogLevel = LogLevelTranslator.Translate(options.MinLevel);

			// Update global filter rules
			ICollection<LoggerFilterRule> optionsFilterRules = options.Rules;
			ICollection<IFilterRule> configFilterRules = configuration.Filters;
			foreach (var kvp in this.filterRuleMap)
			{
				configFilterRules.Remove(kvp.Value);
			}
			this.filterRuleMap.Clear();
			foreach (var optionsRule in optionsFilterRules)
			{
				Func<string, string, LogLevel, bool> filter = null;
				if (optionsRule.Filter != null)
				{
					filter = (_adapterName, _loggerName, _logLevel) => optionsRule.Filter(_adapterName, _loggerName, LogLevelTranslator.Translate(_logLevel));
				}
				var configRule = new FilterRule(
					optionsRule.ProviderName,
					optionsRule.CategoryName,
					LogLevelTranslator.Translate(optionsRule.LogLevel),
					filter);
				this.filterRuleMap.Add(optionsRule, configRule);
				configFilterRules.Add(configRule);
			}

			// Update adapter rules
			this.loggingAdapter.UpdateLoggerSettings();

			this.filterReloadToken = this.filterOptions.OnChange(this.ReloadFilterOptions);
		}

		#endregion
	}
}