namespace Neusta.Shared.DataAccess.UnitOfWork.Factory
{
	using System;
	using Neusta.Shared.ObjectProvider;

	internal sealed class ObjectProviderUnitOfWorkServiceFactory : IUnitOfWorkServiceFactory
	{
		private readonly IObjectProvider objectProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectProviderUnitOfWorkServiceFactory"/> class.
		/// </summary>
		public ObjectProviderUnitOfWorkServiceFactory(IObjectProvider objectProvider)
		{
			this.objectProvider = objectProvider;
		}

		#region Implementation of IUnitOfWorkServiceFactory

		/// <summary>
		/// Requests a data context of the specified type.
		/// </summary>
		public IDataContext CreateDataContext(IUnitOfWork unitOfWork, Type dataContextType)
		{
			return this.objectProvider.GetInstance(dataContextType, new object[] { unitOfWork }) as IDataContext;
		}

		/// <summary>
		/// Creates a data repository of the specified type.
		/// </summary>>
		public IDataRepository CreateDataRepository(IUnitOfWork unitOfWork, Type dataRepositoryType)
		{
			return this.objectProvider.GetInstance(dataRepositoryType, new object[] { unitOfWork }) as IDataRepository;
		}

		/// <summary>
		/// Creates a service instance of the specified type.
		/// </summary>
		public object CreateServiceInstance(IUnitOfWork unitOfWork, Type serviceType)
		{
			return this.objectProvider.GetInstance(serviceType, new object[] { unitOfWork });
		}

		#endregion
	}
}