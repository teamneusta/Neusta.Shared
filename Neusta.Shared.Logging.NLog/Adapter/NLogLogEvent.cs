namespace Neusta.Shared.Logging.NLog.Adapter
{
	using System;
	using System.Diagnostics;
	using global::NLog;

	internal sealed class NLogLogEvent : LogEventInfo, ILogEvent
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLogEvent"/> class.
		/// </summary>
		public NLogLogEvent(LogLevel level, string loggerName, string message, Exception exception, params object[] args)
			: base(level, loggerName, message)
		{
			this.Exception = exception;
			this.Parameters = args;
		}

		#region Implementation of ILogEvent

		long ILogEvent.SequenceID
		{
			[DebuggerStepThrough]
			get { return base.SequenceID; }
		}

		global::Neusta.Shared.Logging.LogLevel ILogEvent.Level
		{
			[DebuggerStepThrough]
			get { return base.Level.Translate(); }
			[DebuggerStepThrough]
			set { base.Level = value.Translate(); }
		}

		string ILogEvent.GetFormattedMessage() => this.FormattedMessage;

		#endregion
	}
}