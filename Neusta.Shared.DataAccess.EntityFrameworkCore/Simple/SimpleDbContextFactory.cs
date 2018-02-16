namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Simple
{
	using Microsoft.EntityFrameworkCore;
	using Neusta.Shared.DataAccess.EntityFrameworkCore.Factory;

	public sealed class SimpleDbContextFactory<TDbContext> : BaseDbContextFactory<SimpleDbContextFactory<TDbContext>, TDbContext>
		where TDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleDbContextFactory{TDbContext}"/> class.
		/// </summary>
		public SimpleDbContextFactory()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleDbContextFactory{TDbContext}"/> class.
		/// </summary>
		public SimpleDbContextFactory(DbContextOptions options)
			: base(options)
		{
		}
	}
}