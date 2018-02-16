namespace Neusta.Shared.DataAccess.UnitOfWork.Factory
{
	using System;

	internal sealed class ServiceProviderUnitOfWorkServiceFactory : IUnitOfWorkServiceFactory
	{
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceProviderUnitOfWorkServiceFactory"/> class.
		/// </summary>
		public ServiceProviderUnitOfWorkServiceFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		#region Implementation of IUnitOfWorkServiceFactory

		/// <summary>
		/// Requests a data context of the specified type.
		/// </summary>
		public IDataContext CreateDataContext(IUnitOfWork unitOfWork, Type dataContextType)
		{
			return this.serviceProvider.GetService(dataContextType) as IDataContext;
		}

		/// <summary>
		/// Creates a data repository of the specified type.
		/// </summary>>
		public IDataRepository CreateDataRepository(IUnitOfWork unitOfWork, Type dataRepositoryType)
		{
			return this.serviceProvider.GetService(dataRepositoryType) as IDataRepository;
		}

		/// <summary>
		/// Creates a service instance of the specified type.
		/// </summary>
		public object CreateServiceInstance(IUnitOfWork unitOfWork, Type serviceType)
		{
			return this.serviceProvider.GetService(serviceType);
		}

		#endregion
	}
}