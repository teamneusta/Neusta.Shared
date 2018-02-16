namespace Neusta.Shared.DataAccess
{
	public interface IDataContext<TDataContext> : IDataContext
		where TDataContext : IDataContext<TDataContext>
	{
	}
}