namespace Neusta.Shared.Logging.Adapter.Multi
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Base;

	internal sealed class MultiLogger : BaseSimplifiedAdapterLogger<MultiLoggingAdapter>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ILogger[] loggers;

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLogger"/> class.
		/// </summary>
		public MultiLogger(MultiLoggingAdapter adapter, string name)
			: base(adapter, name)
		{
			this.loggers = adapter.LoggingAdapters.Select(x => x.GetLogger(name)).ToArray();
		}

		/// <summary>
		/// Gets the loggers.
		/// </summary>
		[PublicAPI]
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public IEnumerable<ILogger> Loggers
		{
			[DebuggerStepThrough]
			get { return this.loggers; }
		}

		#region Overrides of BaseSimplifiedLogger

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		public override bool IsEnabled(LogLevel level)
		{
			if (!base.IsEnabled(level))
			{
				return false;
			}
			return this.loggers.Any(logger => logger.IsEnabled(level));
		}

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		protected override void InternalLog(ILogEvent logEvent)
		{
			foreach (ILogger logger in this.loggers)
			{
				logger.Log(logEvent);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		protected override void InternalLog(LogLevel level, string message, Exception exception)
		{
			foreach (ILogger logger in this.loggers)
			{
				logger.LogException(level, message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		protected override void InternalLog(LogLevel level, string message)
		{
			foreach (ILogger logger in this.loggers)
			{
				logger.Log(level, message);
			}
		}

		#endregion
	}
}