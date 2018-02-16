namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using System.Data.Entity;
	using JetBrains.Annotations;

	public interface ITenantDbContextFactory<TDbContext> : IDbContextFactory<TDbContext>
		where TDbContext : DbContext
	{
		/// <summary>
		/// Gets a value indicating whether the tenant filter should be enabled.
		/// </summary>
		[PublicAPI]
		bool EnableTenantFilter { get; }
	}
}