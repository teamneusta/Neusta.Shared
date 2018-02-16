namespace Neusta.Shared.Logging.Adapter.Debug
{
	using System;
	using Neusta.Shared.Logging.Base;

	internal class DebugLogger : BaseSimplifiedAdapterLogger<DebugLoggingAdapter>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DebugLogger"/> class.
		/// </summary>
		public DebugLogger(DebugLoggingAdapter adapter, string name)
			: base(adapter, name)
		{
		}

		#region Overrides of BaseSimplifiedLogger

		public override ILogEvent CreateLogEvent(LogLevel level, string message, Exception exception, params object[] args)
		{
			return new LogEvent(this, level, message, exception, args);
		}

		/// <summary>
		/// Writes the specified diagnostic message.
		/// </summary>
		protected override void InternalLog(ILogEvent logEvent)
		{
			System.Diagnostics.Debug.WriteLine(logEvent.LoggerName + " - " + logEvent.Level.ToString() + ": " + logEvent.GetFormattedMessage());
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		protected override void InternalLog(LogLevel level, string message, Exception exception)
		{
			System.Diagnostics.Debug.WriteLine(this.Name + " - " + level.ToString() + ": " + message);
			System.Diagnostics.Debug.WriteLine(exception.ToString());
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		protected override void InternalLog(LogLevel level, string message)
		{
			System.Diagnostics.Debug.WriteLine(this.Name + " - " + level.ToString() + ": " + message);
		}

		#endregion
	}
}