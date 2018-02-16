// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Configuration.Helper;

	// ReSharper disable once InconsistentNaming
	public static class ILoggingBuilderExtensions
	{
		/// <summary>
		/// Configure the logging builder.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder Configure(this ILoggingBuilder builder, Action<ILoggingConfigurationSyntax> configure)
		{
			if (configure != null)
			{
				var syntax = new RootSyntaxHelper(builder, builder.Configuration);
				configure(syntax);
			}
			return builder;
		}

		/// <summary>
		/// Configure the logging builder.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder Configure(this ILoggingBuilder builder, ILoggingConfiguration configuration)
		{
			if (configuration != null)
			{
				builder.UpdateConfiguration(configuration);
			}
			return builder;
		}
	}
}