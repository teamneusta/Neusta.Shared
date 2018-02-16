namespace Neusta.Shared.DataAccess.EntityFramework
{
	public interface IEntityFrameworkDataRepository<TDataContext> : IDataRepository<TDataContext>
		where TDataContext : IDataContext
	{
	}
}