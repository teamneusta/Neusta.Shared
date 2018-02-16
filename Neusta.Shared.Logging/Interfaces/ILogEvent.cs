namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface ILogEvent
	{
		/// <summary>
		/// Gets the sequence ID.
		/// </summary>
		[PublicAPI]
		long SequenceID { get; }

		/// <summary>
		/// Gets the timestamp of the logging event.
		/// </summary>
		[PublicAPI]
		DateTime TimeStamp { get; }

		/// <summary>
		/// Gets the logger name.
		/// </summary>
		[PublicAPI]
		string LoggerName { get; set; }

		/// <summary>
		/// Gets the level of the logging event.
		/// </summary>
		[PublicAPI]
		LogLevel Level { get; set; }

		/// <summary>
		/// Gets the log message including any parameter placeholders.
		/// </summary>
		[PublicAPI]
		string Message { get; set; }

		/// <summary>
		/// Gets the parameter values or null if no parameters have been specified.
		/// </summary>
		[PublicAPI]
		object[] Parameters { get; set; }

		/// <summary>
		/// Gets the format provider.
		/// </summary>
		[PublicAPI]
		IFormatProvider FormatProvider { get; set; }

		/// <summary>
		/// Gets the exception information.
		/// </summary>
		[PublicAPI]
		Exception Exception { get; set; }

		/// <summary>
		/// Gets a value indicating whether this event has properties.
		/// </summary>
		[PublicAPI]
		bool HasProperties { get; }

		/// <summary>
		/// Gets the dictionary of per-event context properties.
		/// </summary>
		[PublicAPI]
		IDictionary<object, object> Properties { get; }

		/// <summary>
		/// Gets the formatted message.
		/// </summary>
		[PublicAPI]
		string GetFormattedMessage();
	}
}