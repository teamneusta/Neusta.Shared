namespace Neusta.Shared.AspNetCore.Extensions.Logging
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging.Configuration;
	using Microsoft.Extensions.Options;

	[UsedImplicitly]
	internal class LoggerOptionsSetup : ConfigureFromConfigurationOptions<LoggerOptions>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoggerOptionsSetup"/> class.
		/// </summary>
		public LoggerOptionsSetup(ILoggerProviderConfiguration<LoggerProvider> providerConfiguration)
			: base(providerConfiguration.Configuration)
		{
		}
	}
}