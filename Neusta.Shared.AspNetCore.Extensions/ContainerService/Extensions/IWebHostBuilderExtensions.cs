// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	// ReSharper disable once InconsistentNaming
	public static partial class IWebHostBuilderExtensions
	{
		/// <summary>
		/// Use the default <see cref="IContainerAdapter" /> for dependency injection.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseDefaultContainerAdapter(this IWebHostBuilder webHostBuilder)
		{
			if (ContainerAdapter.IsRegistered)
			{
				return webHostBuilder.UseContainerAdapter(ContainerAdapter.Default);
			}
			return webHostBuilder;
		}

		/// <summary>
		/// Use the specified <see cref="IContainerAdapter" /> for dependency injection.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseContainerAdapter(this IWebHostBuilder webHostBuilder, IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(webHostBuilder, nameof(webHostBuilder));
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			return webHostBuilder.ConfigureServices(services => services.UseContainerAdapter(containerAdapter));
		}

		/// <summary>
		/// Use the specified <see cref="IContainerBuilder" /> for dependency injection.
		/// </summary>
		[PublicAPI]
		public static IWebHostBuilder UseContainerBuilder(this IWebHostBuilder webHostBuilder, IContainerBuilder containerBuilder)
		{
			Guard.ArgumentNotNull(webHostBuilder, nameof(webHostBuilder));
			Guard.ArgumentNotNull(containerBuilder, nameof(containerBuilder));

			return webHostBuilder.ConfigureServices(services => services.UseContainerBuilder(containerBuilder));
		}
	}
}