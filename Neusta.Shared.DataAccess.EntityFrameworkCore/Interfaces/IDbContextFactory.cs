namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	public interface IDbContextFactory<TDbContext>
		where TDbContext : DbContext
	{
		/// <summary>
		/// Gets the options.
		/// </summary>
		[PublicAPI]
		DbContextOptions Options { get; }

		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		[PublicAPI]
		TDbContext CreateDbContext();

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