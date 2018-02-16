using System;

namespace Neusta.Shared.Logging.Configuration.Filter
{
	using System.Diagnostics;

	public sealed class FilterRule : IFilterRule
	{
		public static readonly Func<string, string, LogLevel, bool> True = (arg1, arg2, arg3) => true;
		public static readonly Func<string, string, LogLevel, bool> False = (arg1, arg2, arg3) => false;

		private readonly string adapterName;
		private readonly string loggerName;
		private readonly LogLevel logLevel;
		private readonly Func<string, string, LogLevel, bool> filter;
		private object tag;

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterRule"/> class.
		/// </summary>
		public FilterRule(string adapterName, string loggerName, LogLevel logLevel, Func<string, string, LogLevel, bool> filter)
		{
			this.adapterName = adapterName;
			this.loggerName = loggerName;
			this.logLevel = logLevel;
			this.filter = filter;
		}

		/// <summary>
		/// Gets the adapter name this rule applies to.
		/// </summary>
		public string AdapterName
		{
			[DebuggerStepThrough]
			get { return this.adapterName; }
		}

		/// <summary>
		/// Gets the logger name this rule applies to.
		/// </summary>
		public string LoggerName
		{
			[DebuggerStepThrough]
			get { return this.loggerName; }
		}

		/// <summary>
		/// Gets the minimum <see cref="LogLevel"/> of messages.
		/// </summary>
		public LogLevel LogLevel
		{
			[DebuggerStepThrough]
			get { return this.logLevel; }
		}

		/// <summary>
		/// Gets the filter delegate that would be applied to messages that passed the <see cref="LogLevel"/>.
		/// </summary>
		public Func<string, string, LogLevel, bool> Filter
		{
			[DebuggerStepThrough]
			get { return this.filter; }
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
	}
}
