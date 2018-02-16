namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;

	public interface IRepositoryProvider
	{
		/// <summary>
		/// Gets the data repository.
		/// </summary>
		[PublicAPI]
		IDataRepository GetRepository(Type dataRepositoryType);

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		[PublicAPI]
		TDataRepository GetRepository<TDataRepository>()
			where TDataRepository : IDataRepository;
	}
}