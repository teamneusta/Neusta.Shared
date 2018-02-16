namespace Neusta.Shared.Logging.Base
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using Neusta.Shared.Logging.Adapter;
	using Neusta.Shared.Logging.Adapter.Null;

	public abstract class BaseLoggingAdapter : IDisposable, ILoggingAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly ILogger nullLogger = new NullLogger();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string name;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ILoggingConfiguration configuration;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ConcurrentDictionary<string, ILogger> knownLoggers = new ConcurrentDictionary<string, ILogger>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LogLevel globalThreshold = LogLevel.All;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int disableLevel;
		private EventHandler<EventArgs> configurationChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseLoggingAdapter"/> class.
		/// </summary>
		protected BaseLoggingAdapter(ILoggingConfiguration configuration)
		{
			this.name = this.GetType().Name;
			this.configuration = configuration;
		}

		#region Implementation of ILoggerAdapter

		/// <summary>
		/// Gets the adapter name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		public virtual ILoggingConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		/// <summary>
		/// Gets a value indicating whether the logger is configured.
		/// </summary>
		public virtual bool IsInitialized
		{
			[DebuggerStepThrough]
			get { return true; }
		}

		/// <summary>
		/// Gets or sets the global threshold.
		/// </summary>
		public virtual LogLevel GlobalThreshold
		{
			[DebuggerStepThrough]
			get { return this.globalThreshold; }
			[DebuggerStepThrough]
			set
			{
				if (this.globalThreshold != value)
				{
					this.globalThreshold = value;
					this.UpdateLoggerSettings();
				}
			}
		}

		/// <summary>
		/// Occurs when the configuration changed.
		/// </summary>
		public event EventHandler<EventArgs> ConfigurationChanged
		{
			[DebuggerStepThrough]
			add { this.configurationChanged += value; }
			[DebuggerStepThrough]
			remove { this.configurationChanged -= value; }
		}

		/// <summary>
		/// Initializes this adapter instance.
		/// </summary>
		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Decreases the log enable counter and if it reaches -1 the logs are disabled.
		/// </summary>
		public virtual void DisableLogging()
		{
			if (Interlocked.Increment(ref this.disableLevel) == 1)
			{
				this.UpdateLoggerSettings();
			}
		}

		/// <summary>
		/// Increases the log enable counter and if it reaches 0 the logs are disabled.
		/// </summary>
		public virtual void EnableLogging()
		{
			if (Interlocked.Decrement(ref this.disableLevel) == 0)
			{
				this.UpdateLoggerSettings();
			}
		}

		/// <summary>
		/// Returns <see langword="true"/> if logging is currently enabled.
		/// </summary>
		public bool IsLoggingEnabled()
		{
			return this.disableLevel == 0;
		}

		/// <summary>
		/// Returns <see langword="true"/> if logging for the given <see cref="LogLevel"/> is enabled by the global threshold.
		/// </summary>
		public virtual bool IsLoggingEnabled(LogLevel logLevel)
		{
			return (this.disableLevel == 0) && (logLevel >= this.globalThreshold);
		}

		/// <summary>
		/// Updates the settings on all loggers.
		/// </summary>
		public virtual void UpdateLoggerSettings()
		{
			this.configurationChanged?.Invoke(this, EventArgs.Empty);
			foreach (var logger in this.knownLoggers.Values)
			{
				logger.UpdateLoggerSettings();
			}
		}

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		public virtual ILogger GetNullLogger()
		{
			return nullLogger;
		}

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		public virtual ILogger<T> GetNullLogger<T>()
		{
			int hashCode = RuntimeHelpers.GetHashCode(typeof(T));
			string key = hashCode + @"-null";
			return this.knownLoggers.GetOrAdd(key, value => new GenericLogger<T>(nullLogger)) as ILogger<T>;
		}

		/// <summary>
		/// Gets the specified named logger.
		/// </summary>
		public virtual ILogger GetLogger(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Logger name not specified", nameof(name));
			}
			string key = @"|" + name;
			return this.knownLoggers.GetOrAdd(key, value => this.CreateLogger(name));
		}

		/// <summary>
		/// Gets the specified named logger.
		/// </summary>
		public virtual ILogger<T> GetLogger<T>()
		{
			string name = LogManager.GetLoggerTypeName(typeof(T));
			int hashCode = RuntimeHelpers.GetHashCode(typeof(T));
			string key = hashCode + @"|" + name;
			return this.knownLoggers.GetOrAdd(key, value => this.CreateLogger<T>(name)) as ILogger<T>;
		}

		/// <summary>
		/// Gets the specified named logger.
		/// </summary>
		public virtual ILogger<T> GetLogger<T>(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Logger name not specified", nameof(name));
			}
			int hashCode = RuntimeHelpers.GetHashCode(typeof(T));
			string key = hashCode + @"|" + name;
			return this.knownLoggers.GetOrAdd(key, value => this.CreateLogger<T>(name)) as ILogger<T>;
		}

		/// <summary>
		/// Gets all known named loggers.
		/// </summary>
		public virtual IEnumerable<KeyValuePair<string, ILogger>> GetKnownLoggers()
		{
			return this.knownLoggers;
		}

		#endregion

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		#endregion

		#region Abstract/Protected Methods

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		protected virtual void Dispose()
		{
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected abstract ILogger CreateLogger(string name);

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected virtual ILogger<T> CreateLogger<T>(string name)
		{
			return new GenericLogger<T>(this.CreateLogger(name));
		}

		#endregion
	}
}