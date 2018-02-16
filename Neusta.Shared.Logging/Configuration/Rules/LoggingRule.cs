namespace Neusta.Shared.Logging.Configuration.Rules
{
	using System.Diagnostics;

	internal sealed class LoggingRule : ILoggingRule
	{
		private readonly LogLevel minLevel;
		private readonly LogLevel maxLevel;
		private readonly string targetName;
		private readonly string loggerNamePattern;
		private readonly bool final;
		private object tag;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingRule"/> class.
		/// </summary>
		public LoggingRule(LogLevel minLevel, LogLevel maxLevel, string targetName, string loggerNamePattern, bool final)
		{
			this.minLevel = minLevel;
			this.maxLevel = maxLevel;
			this.targetName = targetName;
			this.loggerNamePattern = loggerNamePattern;
			this.final = final;
		}

		#region Implementation of ILoggingRule

		/// <summary>
		/// Gets the minimum level.
		/// </summary>
		public LogLevel MinLevel
		{
			[DebuggerStepThrough]
			get { return this.minLevel; }
		}

		/// <summary>
		/// Gets the maximum level.
		/// </summary>
		public LogLevel MaxLevel
		{
			[DebuggerStepThrough]
			get { return this.maxLevel; }
		}

		/// <summary>
		/// Gets the name of the target.
		/// </summary>
		public string TargetName
		{
			[DebuggerStepThrough]
			get { return this.targetName; }
		}

		/// <summary>
		/// Gets the logger name pattern.
		/// </summary>
		public string LoggerNamePattern
		{
			[DebuggerStepThrough]
			get { return this.loggerNamePattern; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="LoggingRule"/> is a final rule.
		/// </summary>
		public bool Final
		{
			[DebuggerStepThrough]
			get { return this.final; }
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		public object Tag
		{
			[DebuggerStepThrough]
			get { return this.tag; }
			[DebuggerStepThrough]
			set { this.tag = value; }
		}

		#endregion
	}
}