namespace Neusta.Shared.DataAccess
{
	using System;

	public interface IDataRepository<TDataContext> : IDataRepository, IDataContextOwner<TDataContext>
		where TDataContext : IDataContext
	{
	}
}