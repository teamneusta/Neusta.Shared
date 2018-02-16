// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Logging;
	using Neusta.Shared.AspNetCore.Extensions.ContainerService;
	using Neusta.Shared.AspNetCore.Extensions.Logging;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.DataAccess;
	using Neusta.Shared.DataAccess.UnitOfWork;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Logging.Adapter;
	using Neusta.Shared.ObjectProvider;

	using ILoggingBuilder = Neusta.Shared.Logging.ILoggingBuilder;

	// ReSharper disable once InconsistentNaming
	public static partial class IServiceCollectionExtensions
	{
		#region UnitOfWork

		[PublicAPI]
		public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
		{
			Guard.ArgumentNotNull(services, nameof(services));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}

		#endregion
	}
}
