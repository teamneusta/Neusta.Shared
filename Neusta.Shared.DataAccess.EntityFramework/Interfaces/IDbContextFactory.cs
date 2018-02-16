namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.Data.Common;
	using System.Data.Entity;
	using JetBrains.Annotations;

	public interface IDbContextFactory<TDbContext>
		where TDbContext : DbContext
	{
		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		[PublicAPI]
		TDbContext CreateDbContext();

		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		[PublicAPI]
		TDbContext CreateDbContext(DbConnection connection, bool ownsConnection);

		/// <summary>
		/// Rents a <see cref="TDbContext"/> from the factory.
		/// </summary>
		[PublicAPI]
		TDbContext RentDbContext();

		/// <summary>
		/// Returns the given a <see cref="TDbContext"/> to the factory.
		/// </summary>
		[PublicAPI]
		void ReturnDbContext(TDbContext dbContext);
	}
}