namespace Neusta.Shared.Logging.Configuration.Helper
{
	using System.Diagnostics;

	internal sealed class RootSyntaxHelper : ILoggingConfigurationSyntax,
		IToSyntaxResult
	{
		private readonly ILoggingBuilder builder;
		private readonly ILoggingConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="RootSyntaxHelper" /> class.
		/// </summary>
		public RootSyntaxHelper(ILoggingBuilder builder, ILoggingConfiguration configuration)
		{
			this.builder = builder;
			this.configuration = configuration;
		}

		/// <summary>
		/// Gets the <see cref="ILoggingBuilder" />.
		/// </summary>
		public ILoggingBuilder Builder
		{
			[DebuggerStepThrough]
			get { return this.builder; }
		}

		/// <summary>
		/// Gets the <see cref="ILoggingConfiguration" />.
		/// </summary>
		public ILoggingConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		#region Implementation of ILoggingBuilderSyntax

		/// <summary>
		/// Configures the minimum logging level.
		/// </summary>
		public ILogLevelSyntax<ILoggingConfigurationSyntax> MinimumLevel
		{
			get
			{
				return new LogLevelSyntaxHelper<ILoggingConfigurationSyntax>(this.configuration, this,
					delegate (ILoggingConfiguration _configuration, LogLevel _logLevel)
					{
						_configuration.MinimumLogLevel = _logLevel;
					});
			}
		}

		/// <summary>
		/// Configures the logging targets.
		/// </summary>
		public IToSyntax To
		{
			get { return new ToSyntaxHelper(this.configuration, this); }
		}

		#endregion
	}
}