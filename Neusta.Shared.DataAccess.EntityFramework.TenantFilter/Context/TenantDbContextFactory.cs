namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Context
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Factory;

	[PublicAPI]
	public abstract class TenantDbContextFactory<TDbContextFactory, TDbContext> : BaseDbContextFactory<TDbContextFactory, TDbContext>, ITenantDbContextFactory<TDbContext>
		where TDbContextFactory : TenantDbContextFactory<TDbContextFactory, TDbContext>
		where TDbContext : TenantDbContext, new()
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TenantDbContextFactory{TDbContextFactory, TDbContext}"/> class.
		/// </summary>
		protected TenantDbContextFactory()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TenantDbContextFactory{TDbContextFactory, TDbContext}"/> class.
		/// </summary>
		protected TenantDbContextFactory(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		#region Implementation of ITenantDbContextFactory<TDbContext>

		public bool EnableTenantFilter { get; }

		#endregion

		#region Overrides of BaseDbContextFactory<TDbContextFactory,TDbContext>

		public override TDbContext CreateDbContext()
		{
			TDbContext context = base.CreateDbContext();
			if (this.EnableTenantFilter)
			{
				context.EnableTenantFilter();
			}
			return context;
		}

		public override TDbContext RentDbContext()
		{
			return base.RentDbContext();
		}

		#endregion
	}
}