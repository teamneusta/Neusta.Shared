namespace Neusta.Shared.Logging.Adapter
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	internal sealed class LogEvent : ILogEvent, IEnumerable<KeyValuePair<object, object>>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly IEnumerable<KeyValuePair<object, object>> emptyProperties = Enumerable.Empty<KeyValuePair<object, object>>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static long sequenceCounter;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly long sequenceID;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly DateTime timeStamp;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string loggerName;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LogLevel level;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string message;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Exception exception;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] parameters;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IDictionary<object, object> properties;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IFormatProvider formatProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogEvent"/> class.
		/// </summary>
		public LogEvent(ILogger logger, LogLevel level, string message, Exception exception, object[] args)
		{
			this.loggerName = logger.Name;
			this.timeStamp = DateTime.UtcNow;
			this.sequenceID = Interlocked.Increment(ref sequenceCounter);
			this.level = level;
			this.message = message;
			this.exception = exception;
			this.parameters = args;
		}

		#region Implementation of ILogEvent

		/// <summary>
		/// Gets the sequence ID.
		/// </summary>
		public long SequenceID
		{
			[DebuggerStepThrough]
			get { return this.sequenceID; }
		}

		/// <summary>
		/// Gets the timestamp of the logging event.
		/// </summary>
		public DateTime TimeStamp
		{
			[DebuggerStepThrough]
			get { return this.timeStamp; }
		}

		/// <summary>
		/// Gets the logger name.
		/// </summary>
		public string LoggerName
		{
			[DebuggerStepThrough]
			get { return this.loggerName; }
			[DebuggerStepThrough]
			set { this.loggerName = value; }
		}

		/// <summary>
		/// Gets the level of the logging event.
		/// </summary>
		public LogLevel Level
		{
			[DebuggerStepThrough]
			get { return this.level; }
			[DebuggerStepThrough]
			set { this.level = value; }
		}

		/// <summary>
		/// Gets the log message including any parameter placeholders.
		/// </summary>
		public string Message
		{
			[DebuggerStepThrough]
			get { return this.message; }
			[DebuggerStepThrough]
			set { this.message = value; }
		}

		/// <summary>
		/// Gets the parameter values or null if no parameters have been specified.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object[] Parameters
		{
			[DebuggerStepThrough]
			get { return this.parameters; }
			[DebuggerStepThrough]
			set { this.parameters = value; }
		}

		/// <summary>
		/// Gets the format provider.
		/// </summary>
		public IFormatProvider FormatProvider
		{
			[DebuggerStepThrough]
			get { return this.formatProvider; }
			[DebuggerStepThrough]
			set { this.formatProvider = value; }
		}

		/// <summary>
		/// Gets the exception information.
		/// </summary>
		public Exception Exception
		{
			[DebuggerStepThrough]
			get { return this.exception; }
			[DebuggerStepThrough]
			set { this.exception = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this event has properties.
		/// </summary>
		public bool HasProperties
		{
			[DebuggerStepThrough]
			get { return this.properties != null && this.properties.Any(); }
		}

		/// <summary>
		/// Gets the dictionary of per-event context properties.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public IDictionary<object, object> Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new Dictionary<object, object>();
				}
				return this.properties;
			}
		}

		/// <summary>
		/// Gets the formatted message.
		/// </summary>
		public string GetFormattedMessage()
		{
			var formatProvider = this.formatProvider ?? CultureInfo.CurrentCulture;
			if (this.parameters != null)
			{
				return string.Format(formatProvider, this.message, this.parameters);
			}
			return string.Format(formatProvider, this.message);
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary
		/// >Returns an enumerator that iterates through a collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable<KeyValuePair<object,object>>

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			if (this.properties != null)
			{
				return this.properties.GetEnumerator();
			}
			return emptyProperties.GetEnumerator();
		}

		#endregion
	}
}