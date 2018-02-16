namespace Neusta.Shared.Logging.NLog.Adapter
{
	using System;
	using System.Runtime.CompilerServices;
	using global::NLog;
	using Neusta.Shared.Logging.Base;

	internal class NLogLogger : BaseSimplifiedAdapterLogger<NLogLoggingAdapter>
	{
		private readonly global::NLog.ILogger nlogLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLogger"/> class.
		/// </summary>
		public NLogLogger(NLogLoggingAdapter adapter, global::NLog.ILogger nlogLogger, string name)
			: base(adapter, name)
		{
			this.nlogLogger = nlogLogger;

			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateIsEnabledValues();
		}

		#region Overrides of BaseSimplifiedLogger

		/// <summary>
		/// Creates an empty log event.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override ILogEvent CreateLogEvent(global::Neusta.Shared.Logging.LogLevel level, string message, Exception exception, params object[] args)
		{
			return new NLogLogEvent(level.Translate(), this.Name, message, exception, args);
		}

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool IsEnabled(global::Neusta.Shared.Logging.LogLevel level)
		{
			return this.nlogLogger.IsEnabled(level.Translate()) && base.IsEnabled(level);
		}

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(ILogEvent logEvent)
		{
			if (logEvent is LogEventInfo typedLogEvent)
			{
				this.nlogLogger.Log(logEvent.Level.Translate(), typedLogEvent);
			}
			else if (logEvent.HasProperties)
			{
				var translatedLogEvent = new NLogLogEvent(logEvent.Level.Translate(), this.Name, logEvent.Message, logEvent.Exception, logEvent.Parameters);
				translatedLogEvent.FormatProvider = logEvent.FormatProvider;
				foreach (var kvp in logEvent.Properties)
				{
					translatedLogEvent.Properties.Add(kvp);
				}
				this.nlogLogger.Log(logEvent.Level.Translate(), translatedLogEvent);
			}
			else
			{
				base.InternalLog(logEvent);
			}
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message, Exception exception)
		{
			if (exception != null)
			{
				this.nlogLogger.Log(level.Translate(), exception, message);
			}
			else
			{
				this.nlogLogger.Log(level.Translate(), message);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message)
		{
			this.nlogLogger.Log(level.Translate(), (Exception)null, message);
		}

		/// <summary>
		/// Updates the IsEnabled values.
		/// </summary>
		protected override void UpdateIsEnabledValues()
		{
			if (this.nlogLogger != null)
			{
				base.UpdateIsEnabledValues();
			}
		}

		#endregion
	}
}