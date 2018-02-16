namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Simple
{
	using Microsoft.EntityFrameworkCore;

	public class SimpleDataContext<TDbContext> : EntityFrameworkDataContext<SimpleDataContext<TDbContext>, TDbContext>
		where TDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleDataContext{TDbContext}"/> class.
		/// </summary>
		public SimpleDataContext(SimpleDbContextFactory<TDbContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}
	}
}
