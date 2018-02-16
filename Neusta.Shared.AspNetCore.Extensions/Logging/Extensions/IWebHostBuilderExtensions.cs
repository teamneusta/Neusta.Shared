// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;
	using ILoggingBuilder = Microsoft.Extensions.Logging.ILoggingBuilder;

	// ReSharper disable once InconsistentNaming
	public static partial class IWebHostBuilderExtensions
	{
		/// <summary>
		/// Use this host as target for Neusta.Shared logging output.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseAsSharedLoggingTarget(this IWebHostBuilder webHostBuilder)
		{
			return webHostBuilder.ConfigureServices(services => services.AddSharedLoggingTarget());
		}

		/// <summary>
		/// Use the default <see cref="Neusta.Shared.Logging.ILoggingAdapter" /> for logging.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseDefaultLoggingAdapter(this IWebHostBuilder webHostBuilder)
		{
			return webHostBuilder.UseLoggingAdapter(LoggingAdapter.Default);
		}

		/// <summary>
		/// Use the specified <see cref="Neusta.Shared.Logging.ILoggingAdapter" /> for logging.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseLoggingAdapter(this IWebHostBuilder webHostBuilder, global::Neusta.Shared.Logging.ILoggingAdapter loggingAdapter)
		{
			Guard.ArgumentNotNull(webHostBuilder, nameof(webHostBuilder));
			Guard.ArgumentNotNull(loggingAdapter, nameof(loggingAdapter));

			return webHostBuilder.ConfigureLogging(delegate(WebHostBuilderContext context, ILoggingBuilder builder)
			{
				builder.AddConfiguration(context.Configuration.GetSection("Logging"));
				builder.AddLoggingAdapter(loggingAdapter);
			});
		}

		/// <summary>
		/// Use the specified <see cref="Neusta.Shared.Logging.ILoggingBuilder" /> for logging.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseLoggingBuilder(this IWebHostBuilder webHostBuilder, global::Neusta.Shared.Logging.ILoggingBuilder loggingBuilder)
		{
			Guard.ArgumentNotNull(webHostBuilder, nameof(webHostBuilder));
			Guard.ArgumentNotNull(loggingBuilder, nameof(loggingBuilder));

			return webHostBuilder.ConfigureLogging(delegate (WebHostBuilderContext context, ILoggingBuilder builder)
			{
				builder.AddConfiguration(context.Configuration.GetSection("Logging"));
				builder.AddLoggingBuilder(loggingBuilder);
			});
		}
	}
}