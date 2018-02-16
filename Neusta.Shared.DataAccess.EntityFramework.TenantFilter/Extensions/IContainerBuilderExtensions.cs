using System;

// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter;
	using Neusta.Shared.ObjectProvider.Configuration;

	// ReSharper disable once InconsistentNaming
	public static class IContainerBuilderExtensions
	{
		/// <summary>
		/// Adds a tenant-based unit of work registration with a static tenant ID.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder AddUnitOfWork(this IContainerBuilder builder, long staticTenantID)
		{
			builder.Configuration.ServiceDescriptors
				.RemoveAll<IUnitOfWork>()
				.Add(ServiceDescriptor.Describe<IUnitOfWork, ITenantFilterUnitOfWork>(
					provider => BuildUnitOfWork(provider, staticTenantID),
					ServiceLifetime.PerResolutionRequest));
			return builder;
		}

		/// <summary>
		/// Adds a tenant-based unit of work registration that receives the tenant ID from a callback function.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder AddUnitOfWork(this IContainerBuilder builder, Func<long> tenantIDFunc)
		{
			builder.Configuration.ServiceDescriptors
				.RemoveAll<IUnitOfWork>()
				.Add(ServiceDescriptor.Describe<IUnitOfWork, ITenantFilterUnitOfWork>(
					provider => BuildUnitOfWork(provider, tenantIDFunc()),
					ServiceLifetime.PerResolutionRequest));
			return builder;
		}

		#region Private Methods

		private static ITenantFilterUnitOfWork BuildUnitOfWork(IServiceProvider provider, long tenantID)
		{
			ITenantFilterUnitOfWork unitOfWork = new TenantFilterUnitOfWork(provider);
			unitOfWork.InitializeTenantFilter(tenantID);
			return unitOfWork;
		}

		#endregion
	}
}
