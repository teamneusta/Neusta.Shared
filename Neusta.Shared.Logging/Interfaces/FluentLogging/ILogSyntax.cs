namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections;
	using JetBrains.Annotations;

	public interface ILogSyntax : IFluentSyntax
	{
		/// <summary>
		/// Sets the logger name of the logging event.
		/// </summary>
		ILogSyntaxResult LoggerName(string loggerName);

		/// <summary>
		/// Sets the log message on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message);

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message, object arg0);

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message, object arg0, object arg1);

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message, object arg0, object arg1, object arg2);

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message, object arg0, object arg1, object arg2, object arg3);

		/// <summary>
		/// Sets the log message and parameters for formatting on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Message(string message, params object[] args);

		/// <summary>
		/// Sets the <paramref name="exception"/> information of the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Exception(Exception exception);

		/// <summary>
		/// Sets the format provider.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult FormatProvider(IFormatProvider formatProvider);

		/// <summary>
		/// Sets a per-event context property on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Property(object name, object value);

		/// <summary>
		/// Sets multiple per-event context properties on the logging event.
		/// </summary>
		[PublicAPI]
		ILogSyntaxResult Properties(IDictionary properties);
	}
}