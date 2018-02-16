namespace Neusta.Shared.Logging.CommonLogging.Adapter
{
	using System;
	using System.Globalization;
	using Common.Logging;
	using Common.Logging.Simple;

	internal class SimpleLogger : AbstractSimpleLogger
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleLogger"/> class.
		/// </summary>
		public SimpleLogger(string name)
			: base(name, LogLevel.Off, true, true, true, CultureInfo.InvariantCulture.DateTimeFormat.SortableDateTimePattern)
		{
		}

		#region Overrides of AbstractLogger

		/// <summary>
		/// Actually sends the message to the underlying log system.
		/// </summary>
		protected override void WriteInternal(LogLevel level, object message, Exception exception)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}