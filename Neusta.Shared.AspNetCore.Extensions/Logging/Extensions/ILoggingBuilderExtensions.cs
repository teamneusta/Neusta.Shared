// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Logging.Configuration;
	using Microsoft.Extensions.Options;
	using Neusta.Shared.AspNetCore.Extensions.Logging;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;

	// ReSharper disable once InconsistentNaming
	public static partial class ILoggingBuilderExtensions
	{
		/// <summary>
		/// Adds the default <see cref="Neusta.Shared.Logging.ILoggingAdapter" />.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder AddDefaultLoggingAdapter(this ILoggingBuilder builder)
		{
			return builder.AddLoggingAdapter(LoggingAdapter.Default);
		}

		/// <summary>
		/// Adds the specified <see cref="Neusta.Shared.Logging.ILoggingAdapter" />.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder AddLoggingAdapter(this ILoggingBuilder builder, global::Neusta.Shared.Logging.ILoggingAdapter loggingAdapter)
		{
			Guard.ArgumentNotNull(builder, nameof(builder));
			Guard.ArgumentNotNull(loggingAdapter, nameof(loggingAdapter));

			builder.AddConfiguration();
			builder.ClearProviders();

			builder.Services.Replace(ServiceDescriptor.Singleton<ILoggingAdapter>(loggingAdapter));
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>());
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<LoggerOptions>, LoggerOptionsSetup>());
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<LoggerOptions>, LoggerProviderOptionsChangeTokenSource<LoggerOptions, LoggerProvider>>());

			return builder;
		}

		/// <summary>
		/// Adds the specified <see cref="Neusta.Shared.Logging.ILoggingBuilder" />.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder AddLoggingBuilder(this ILoggingBuilder builder, global::Neusta.Shared.Logging.ILoggingBuilder loggingBuilder)
		{
			Guard.ArgumentNotNull(builder, nameof(builder));
			Guard.ArgumentNotNull(loggingBuilder, nameof(loggingBuilder));

			return builder.AddLoggingAdapter(loggingBuilder.Build());
		}
	}
}