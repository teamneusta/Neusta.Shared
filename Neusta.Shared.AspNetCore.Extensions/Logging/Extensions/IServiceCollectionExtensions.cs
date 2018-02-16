// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Logging;
	using Neusta.Shared.AspNetCore.Extensions.Logging.Adapter;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Logging.Configuration;

	// ReSharper disable once InconsistentNaming
	public static partial class IServiceCollectionExtensions
	{
		/// <summary>
		/// Adds services required for Neusta.Shared logging output targeting.
		/// </summary>
		[PublicAPI]
		public static IServiceCollection AddSharedLoggingTarget(this IServiceCollection services)
		{
			Guard.ArgumentNotNull(services, nameof(services));

			var config = new LoggingConfiguration();
			var builder = LoggingBuilder.From(config);
			builder.RegisterAdapterBuilder<RedirectLoggingAdapterBuilder>();
			var adapter = builder.BuildAsDefault();

			services.AddLogging();
			services.Replace(ServiceDescriptor.Singleton<ILoggerFactory, RedirectLoggerFactory>());
			services.Replace(ServiceDescriptor.Singleton<ILoggingAdapter>(adapter));

			return services;
		}
	}
}