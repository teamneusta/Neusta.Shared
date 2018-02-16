// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using System;
	using global::NLog.Config;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.NLog.Adapter;

	// ReSharper disable once InconsistentNaming
	public static class ILoggingBuilderExtensions
	{
		/// <summary>
		/// Use NLog.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder UseNLog(this ILoggingBuilder builder)
		{
			builder.RegisterAdapterBuilder<NLogLoggingAdapterBuilder>();
			return builder;
		}

		/// <summary>
		/// Use NLog.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder UseNLog(this ILoggingBuilder builder, string configFileName)
		{
			var nlog = builder.RegisterAdapterBuilder<NLogLoggingAdapterBuilder>();
			nlog.ConfigFileName = builder.Configuration.ConfigFileName;
			return builder;
		}

		/// <summary>
		/// Use NLog.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder UseNLog(this ILoggingBuilder builder, string configFileName, Action<LoggingConfiguration> configure)
		{
			var nlog = builder.RegisterAdapterBuilder<NLogLoggingAdapterBuilder>();
			nlog.Configure = configure;
			nlog.ConfigFileName = builder.Configuration.ConfigFileName;
			return builder;
		}

		/// <summary>
		/// Use NLog.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder UseNLog(this ILoggingBuilder builder, Action<LoggingConfiguration> configure)
		{
			var nlog = builder.RegisterAdapterBuilder<NLogLoggingAdapterBuilder>();
			nlog.Configure = configure;
			return builder;
		}
	}
}