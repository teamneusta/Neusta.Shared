namespace Neusta.Shared.Logging.Configuration.Filter
{
	using System;

	internal sealed class FilterRuleSelector
	{
		public static void Select(ILoggingConfiguration configuration, ILoggingAdapter adapter, string loggerName, out LogLevel minLevel, out Func<string, string, LogLevel, bool> filter)
		{
			filter = null;
			minLevel = configuration.MinimumLogLevel;

			IFilterRule current = null;
			string adapterType = adapter.GetType().FullName;
			string adapterName = adapter.Name;
			foreach (var rule in configuration.Filters)
			{
				if (IsBetter(rule, current, adapterType, loggerName) || (!string.IsNullOrEmpty(adapterName) && IsBetter(rule, current, adapterName, loggerName)))
				{
					current = rule;
				}
			}

			if (current != null)
			{
				filter = current.Filter;
				minLevel = current.LogLevel;
			}
		}

		#region Private Methods

		private static bool IsBetter(IFilterRule rule, IFilterRule current, string adapterName, string loggerName)
		{
			// Skip rules with inapplicable adapter or logger names
			if ((rule.AdapterName != null) && (rule.AdapterName != adapterName))
			{
				return false;
			}
			if (rule.LoggerName != null && !loggerName.StartsWith(rule.LoggerName, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			if (current?.AdapterName != null)
			{
				if (rule.AdapterName == null)
				{
					return false;
				}
			}
			else
			{
				// We want to skip logger name check when going from no provider to having provider
				if (rule.AdapterName != null)
				{
					return true;
				}
			}

			if (current?.LoggerName != null)
			{
				if (rule.LoggerName == null)
				{
					return false;
				}

				if (current.LoggerName.Length > rule.LoggerName.Length)
				{
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}