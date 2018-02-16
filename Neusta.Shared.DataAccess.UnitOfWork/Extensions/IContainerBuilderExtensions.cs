using System;

// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess;
	using Neusta.Shared.DataAccess.UnitOfWork;
	using Neusta.Shared.ObjectProvider.Configuration;

	// ReSharper disable once InconsistentNaming
	public static class IContainerBuilderExtensions
	{
		/// <summary>
		/// Adds registration of unit of work.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder AddUnitOfWork(this IContainerBuilder builder)
		{
			builder.Configuration.ServiceDescriptors
				.RemoveAll<IUnitOfWork>()
				.Add(ServiceDescriptor.Describe<IUnitOfWork, UnitOfWork>(ServiceLifetime.PerResolutionRequest));
			return builder;
		}
	}
}
