namespace Neusta.Shared.Logging.Adapter.Debug
{
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Base;

	internal sealed class DebugLoggingAdapter : BaseLoggingAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LogLevel minLevel = LogLevel.Trace;

		/// <summary>
		/// Initializes a new instance of the <see cref="DebugLoggingAdapter"/> class.
		/// </summary>
		public DebugLoggingAdapter(ILoggingConfiguration configuration)
			: base(configuration)
		{
		}

		/// <summary>
		/// Gets or sets the minimum level.
		/// </summary>
		[PublicAPI]
		public LogLevel MinLevel
		{
			[DebuggerStepThrough]
			get { return this.minLevel; }
			[DebuggerStepThrough]
			set
			{
				this.minLevel = value;
				this.UpdateLoggerSettings();
			}
		}

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger CreateLogger(string name)
		{
			var logger = new DebugLogger(this, name);
			return new ForwardingLogger(logger);
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger<T> CreateLogger<T>(string name)
		{
			var logger = new DebugLogger(this, name);
			return new ForwardingLogger<T>(logger);
		}

		#endregion
	}
}