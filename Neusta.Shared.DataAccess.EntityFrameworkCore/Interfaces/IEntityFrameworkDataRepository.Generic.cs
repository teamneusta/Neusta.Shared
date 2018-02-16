namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	public interface IEntityFrameworkDataRepository<TDataContext> : IDataRepository<TDataContext>
		where TDataContext : IDataContext
	{
	}
}