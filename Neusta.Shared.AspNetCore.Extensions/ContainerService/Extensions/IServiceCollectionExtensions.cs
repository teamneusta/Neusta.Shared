// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Neusta.Shared.AspNetCore.Extensions.ContainerService;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	// ReSharper disable once InconsistentNaming
	public static partial class IServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the specified <see cref="IObjectProvider" />.
		/// </summary>
		[PublicAPI]
		public static IServiceCollection AddObjectProvider(this IServiceCollection services, IObjectProvider objectProvider)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(objectProvider, nameof(objectProvider));

			services.Replace(ServiceDescriptor.Singleton<IObjectProvider>(objectProvider));

			return services;
		}

		/// <summary>
		/// Use the specified <see cref="IContainerAdapter" /> for dependency injection.
		/// </summary>
		[PublicAPI]
		public static IServiceCollection UseContainerAdapter(this IServiceCollection services, IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			services.Replace(ServiceDescriptor.Singleton<IContainerAdapter>(containerAdapter));
			services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>, ServiceProviderFactory>());
			services.Replace(ServiceDescriptor.Singleton<IServiceScopeFactory, ServiceScopeFactory>());

			return services;
		}

		/// <summary>
		/// Use the specified <see cref="IContainerBuilder" /> for dependency injection.
		/// </summary>
		[PublicAPI]
		public static IServiceCollection UseContainerBuilder(this IServiceCollection services, IContainerBuilder containerBuilder)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(containerBuilder, nameof(containerBuilder));

			return services.UseContainerAdapter(containerBuilder.Build());
		}
	}
}
