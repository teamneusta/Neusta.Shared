namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface ILoggingAdapter : IDisposable
	{
		/// <summary>
		/// Gets the adapter name.
		/// </summary>
		[PublicAPI]
		string Name { get; }

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		[PublicAPI]
		ILoggingConfiguration Configuration { get; }

		/// <summary>
		/// Gets a value indicating whether the adapter is initialized.
		/// </summary>
		[PublicAPI]
		bool IsInitialized { get; }

		/// <summary>
		/// Gets or sets the global threshold.
		/// </summary>
		[PublicAPI]
		LogLevel GlobalThreshold { get; set; }

		/// <summary>
		/// Occurs when the configuration changed.
		/// </summary>
		[PublicAPI]
		event EventHandler<EventArgs> ConfigurationChanged;

		/// <summary>
		/// Initializes this adapter instance.
		/// </summary>
		[PublicAPI]
		void Initialize();

		/// <summary>
		/// Decreases the log enable counter and if it reaches -1 the logs are disabled.
		/// </summary>
		[PublicAPI]
		void DisableLogging();

		/// <summary>
		/// Increases the log enable counter and if it reaches 0 the logs are disabled.
		/// </summary>
		[PublicAPI]
		void EnableLogging();

		/// <summary>
		/// Returns <see langword="true"/> if logging is currently enabled.
		/// </summary>
		[PublicAPI]
		bool IsLoggingEnabled();

		/// <summary>
		/// Returns <see langword="true"/> if logging for the given <see cref="LogLevel"/> is enabled by the global threshold.
		/// </summary>
		[PublicAPI]
		bool IsLoggingEnabled(LogLevel logLevel);

		/// <summary>
		/// Updates the settings on all loggers.
		/// </summary>
		[PublicAPI]
		void UpdateLoggerSettings();

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		[PublicAPI]
		ILogger GetNullLogger();

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		[PublicAPI]
		ILogger<T> GetNullLogger<T>();

		/// <summary>
		/// Gets the specified logger.
		/// </summary>
		[PublicAPI]
		ILogger GetLogger(string name);

		/// <summary>
		/// Gets the specified logger.
		/// </summary>
		[PublicAPI]
		ILogger<T> GetLogger<T>();

		/// <summary>
		/// Gets the specified logger.
		/// </summary>
		[PublicAPI]
		ILogger<T> GetLogger<T>(string name);

		/// <summary>
		/// Gets all known loggers.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IEnumerable<KeyValuePair<string, ILogger>> GetKnownLoggers();
	}
}