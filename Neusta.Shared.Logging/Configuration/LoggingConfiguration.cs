namespace Neusta.Shared.Logging.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	public class LoggingConfiguration : ILoggingConfiguration
	{
		internal static readonly ILoggingConfiguration Empty = new LoggingConfiguration();

		private readonly List<IFilterRule> filters;
		private readonly List<ILoggingRule> rules;
		private readonly List<ILoggingTarget> targets;
		private string configFileName;
		private LogLevel minimumLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingConfiguration"/> class.
		/// </summary>
		public LoggingConfiguration()
		{
			this.filters = new List<IFilterRule>();
			this.rules = new List<ILoggingRule>();
			this.targets = new List<ILoggingTarget>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingConfiguration"/> class.
		/// </summary>
		public LoggingConfiguration(string configFileName)
			: this()
		{
			this.configFileName = configFileName;
		}

		#region Implementation of ILoggingBuilderData

		/// <summary>
		/// Gets or sets the name of the configuration file.
		/// </summary>
		public string ConfigFileName
		{
			[DebuggerStepThrough]
			get { return this.configFileName; }
			set
			{
				if (!string.IsNullOrEmpty(this.configFileName))
				{
					throw new InvalidOperationException();
				}
				this.configFileName = value;
			}
		}

		/// <summary>
		/// Gets or sets the global log threshold.
		/// </summary>
		public LogLevel MinimumLogLevel
		{
			[DebuggerStepThrough]
			get { return this.minimumLevel; }
			[DebuggerStepThrough]
			set { this.minimumLevel = value; }
		}

		/// <summary>
		/// Gets the filter rules.
		/// </summary>
		public ICollection<IFilterRule> Filters
		{
			[DebuggerStepThrough]
			get { return this.filters; }
		}

		/// <summary>
		/// Gets the logging rules.
		/// </summary>
		public ICollection<ILoggingRule> Rules
		{
			[DebuggerStepThrough]
			get { return this.rules; }
		}

		/// <summary>
		/// Gets the targets.
		/// </summary>
		public ICollection<ILoggingTarget> Targets
		{
			[DebuggerStepThrough]
			get { return this.targets; }
		}

		#endregion
	}
}