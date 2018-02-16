namespace Neusta.Shared.Logging.NLog.Adapter
{
	using System;
	using System.Diagnostics;
	using global::NLog;
	using global::NLog.Config;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Base;

	[UsedImplicitly]
	internal sealed class NLogLoggingAdapter : BaseLoggingAdapter
	{
		private readonly LoggingConfiguration configuration;
		private readonly LogFactory logFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLoggingAdapter"/> class.
		/// </summary>
		public NLogLoggingAdapter(ILoggingConfiguration loggingConfiguration, LoggingConfiguration nlogConfiguration)
			: base(loggingConfiguration)
		{
			this.configuration = nlogConfiguration;
			this.logFactory = new LogFactory(nlogConfiguration);
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		[PublicAPI]
		public new LoggingConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		/// <summary>
		/// Gets the log factory.
		/// </summary>
		[PublicAPI]
		public LogFactory LogFactory
		{
			[DebuggerStepThrough]
			get { return this.logFactory; }
		}

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Gets a value indicating whether the logger is configured.
		/// </summary>
		public override bool IsInitialized
		{
			[DebuggerStepThrough]
			get { return this.configuration != null; }
		}

		/// <summary>
		/// Updates the settings on all loggers.
		/// </summary>
		public override void UpdateLoggerSettings()
		{
			base.UpdateLoggerSettings();
			this.logFactory.ReconfigExistingLoggers();
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected override void Dispose()
		{
			this.logFactory.Flush();
			base.Dispose();
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override global::Neusta.Shared.Logging.ILogger CreateLogger(string name)
		{
			var logger = this.logFactory.GetLogger(name);
			return new NLogLogger(this, logger, name);
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger<T> CreateLogger<T>(string name)
		{
			var logger = this.logFactory.GetLogger(name);
			return new NLogLogger<T>(this, logger, name);
		}

		#endregion
	}
}