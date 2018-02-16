// ReSharper disable once CheckNamespace
namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;

	public interface IUnitOfWorkServiceFactory
	{
		/// <summary>
		/// Creates a data context of the specified type.
		/// </summary>
		[PublicAPI]
		IDataContext CreateDataContext(IUnitOfWork unitOfWork, Type dataContextType);

		/// <summary>
		/// Creates a data repository of the specified type.
		/// </summary>
		[PublicAPI]
		IDataRepository CreateDataRepository(IUnitOfWork unitOfWork, Type dataRepositoryType);

		/// <summary>
		/// Creates a service instance of the specified type.
		/// </summary>
		[PublicAPI]
		object CreateServiceInstance(IUnitOfWork unitOfWork, Type serviceType);
	}
}