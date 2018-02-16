namespace Neusta.Shared.Logging
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
	using System.Threading;
	using JetBrains.Annotations;

	public sealed class LoggingStopwatch : IDisposable
	{
		private readonly ILogger logger;
		private readonly LogLevel logLevel;
		private readonly Stopwatch stopwatch;
		private readonly string messageOnStop;
		private readonly bool execStopOnDispose;
		private bool isDisposed;
		private string levelIndicator;
		private StringBuilder additionalInfo;
		private ValueContainer<int> stopwatchLevel;

		[ThreadStatic]
		private static ValueContainer<int> stopwatchLevelValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingStopwatch"/> class.
		/// </summary>
		public LoggingStopwatch(ILogger logger, LogLevel logLevel)
		{
			this.logger = logger;
			this.logLevel = logLevel;
			this.stopwatch = new Stopwatch();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingStopwatch"/> class.
		/// </summary>
		public LoggingStopwatch(ILogger logger, LogLevel logLevel, string message)
			: this(logger, logLevel)
		{
			this.execStopOnDispose = !string.IsNullOrEmpty(message);
			if (this.execStopOnDispose)
			{
				this.messageOnStop = message;
				this.Start(message);
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingStopwatch"/> class.
		/// </summary>
		public LoggingStopwatch(ILogger logger, LogLevel logLevel, string startMessage, string stopMessage)
			: this(logger, logLevel)
		{
			this.messageOnStop = stopMessage;
			this.execStopOnDispose = !string.IsNullOrEmpty(stopMessage);
			this.Start(startMessage);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="LoggingStopwatch"/> is reclaimed by garbage collection.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		~LoggingStopwatch()
		{
			lock (this)
			{
				if (!this.isDisposed)
				{
					this.Dispose(false);
					this.isDisposed = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the additional information.
		/// </summary>
		[PublicAPI]
		public string AdditionalInfo
		{
			[DebuggerStepThrough]
			get
			{
				if (this.additionalInfo != null)
				{
					return this.additionalInfo.ToString();
				}
				return string.Empty;
			}
			[DebuggerStepThrough]
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.additionalInfo = null;
				}
				else
				{
					this.additionalInfo = new StringBuilder(value);
				}
			}
		}

		/// <summary>
		/// Starts the internal <see cref="Stopwatch"/> and writes the <paramref name="message"/> to the logger.
		/// </summary>
		[PublicAPI]
		public void Start(string message)
		{
			if (!this.stopwatch.IsRunning)
			{
				this.additionalInfo = null;
				if (this.logger != null)
				{
					if (stopwatchLevelValue == null)
					{
						stopwatchLevelValue = new ValueContainer<int>();
					}
					this.stopwatchLevel = stopwatchLevelValue;
					int level = Interlocked.Increment(ref this.stopwatchLevel.Value);
					if (level < 4)
					{
						this.levelIndicator = new string('-', level + 2);
					}
					else
					{
						this.levelIndicator = string.Format("--{0}--", level.ToString(CultureInfo.InvariantCulture));
					}
					this.logger.Log(this.logLevel, string.Format(CultureInfo.InvariantCulture, @" {1}> {0}", message, this.levelIndicator));
				}
				this.stopwatch.Reset();
				this.stopwatch.Start();
			}
		}

		/// <summary>
		/// Stops the internal <see cref="Stopwatch"/> and writes the <paramref name="message"/> to the logger.
		/// </summary>
		[PublicAPI]
		public void Stop(string message)
		{
			if (this.stopwatch.IsRunning)
			{
				this.stopwatch.Stop();
				if (this.logger != null)
				{
					message = string.Format(CultureInfo.InvariantCulture, @" <{1} {0}", message, this.levelIndicator);
					if (message.Contains(@"{0}"))
					{
						message = string.Format(CultureInfo.InvariantCulture, message, this.stopwatch.ElapsedMilliseconds);
					}
					else
					{
						message += string.Format(CultureInfo.InvariantCulture, @", {0}ms", this.stopwatch.ElapsedMilliseconds);
					}
					if (this.additionalInfo != null)
					{
						message += string.Format(" ({0})", this.additionalInfo);
					}
					this.logger.Log(this.logLevel, message);
					int level = Interlocked.Decrement(ref this.stopwatchLevel.Value);
					if ((level == 0) && ReferenceEquals(this.stopwatchLevel, stopwatchLevelValue))
					{
						stopwatchLevelValue = null;
					}
				}
			}
		}

		/// <summary>
		/// First calls <see cref="Stop"/> with the <paramref name="stopMessage"/>, resets the internal <see cref="Stopwatch"/> 
		/// and calls <see cref="Start"/> with the <paramref name="restartMessage"/>.
		/// </summary>
		[PublicAPI]
		public void Restart(string stopMessage, string restartMessage)
		{
			this.Stop(stopMessage);
			this.Start(restartMessage);
		}

		/// <summary>
		/// Adds the additional information.
		/// </summary>
		[PublicAPI]
		public void AddAdditionalInfo(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (this.additionalInfo == null)
				{
					this.additionalInfo = new StringBuilder();
				}
				else
				{
					this.additionalInfo.Append(@", ");
				}
				this.additionalInfo.Append(text);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		public void Dispose()
		{
			lock (this)
			{
				GC.SuppressFinalize(this);
				if (!this.isDisposed)
				{
					this.Dispose(true);
					this.isDisposed = true;
				}
			}
		}

		/// <summary>
		/// Calls the <see cref="Stop"/> method with the specified stopMessage and releases unmanaged and - optionally - managed resources.
		/// </summary>
		private void Dispose(bool disposing)
		{
			if (this.execStopOnDispose)
			{
				this.Stop(this.messageOnStop);
			}
		}

#region Private class

		private sealed class ValueContainer<T>
		{
			public T Value;
		}

#endregion
	}
}
