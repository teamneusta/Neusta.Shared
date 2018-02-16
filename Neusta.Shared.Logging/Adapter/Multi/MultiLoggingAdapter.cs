namespace Neusta.Shared.Logging.Adapter.Multi
{
	using System.Diagnostics;
	using Neusta.Shared.Logging.Base;

	internal sealed class MultiLoggingAdapter : BaseLoggingAdapter
	{
		private readonly ILoggingAdapter[] loggingAdapters;

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLoggingAdapter"/> class.
		/// </summary>
		public MultiLoggingAdapter(ILoggingConfiguration configuration, ILoggingAdapter[] loggingAdapters)
			: base(configuration)
		{
			this.loggingAdapters = loggingAdapters;

			foreach (var adapter in loggingAdapters)
			{
				adapter.ConfigurationChanged += delegate { this.UpdateLoggerSettings(); };
			}
		}

		/// <summary>
		/// Gets the logger adapters.
		/// </summary>
		internal ILoggingAdapter[] LoggingAdapters
		{
			[DebuggerStepThrough]
			get { return this.loggingAdapters; }
		}

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Initializes this adapter instance.
		/// </summary>
		public override void Initialize()
		{
			foreach (var loggerAdapter in this.loggingAdapters)
			{
				loggerAdapter.Initialize();
			}
		}

		/// <summary>
		/// Decreases the log enable counter and if it reaches -1 the logs are disabled.
		/// </summary>
		public override void DisableLogging()
		{
			base.DisableLogging();
			foreach (var loggerAdapter in this.loggingAdapters)
			{
				loggerAdapter.DisableLogging();
			}
		}

		/// <summary>
		/// Increases the log enable counter and if it reaches 0 the logs are disabled.
		/// </summary>
		public override void EnableLogging()
		{
			foreach (var loggerAdapter in this.loggingAdapters)
			{
				loggerAdapter.EnableLogging();
			}
			base.EnableLogging();
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger CreateLogger(string name)
		{
			var logger = new MultiLogger(this, name);
			return new ForwardingLogger(logger);
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger<T> CreateLogger<T>(string name)
		{
			var logger = new MultiLogger(this, name);
			return new ForwardingLogger<T>(logger);
		}

		#endregion
	}
}