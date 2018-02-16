namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Configuration
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Configuration;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Internals;

	[PublicAPI]
	public abstract class SqlAzureTenantDbConfiguration : SqlAzureOptimizedDbConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlAzureTenantDbConfiguration"/> class.
		/// </summary>
		protected SqlAzureTenantDbConfiguration()
		{
			this.AddInterceptor(new TenantCommandInterceptor());
			this.AddInterceptor(new TenantCommandTreeInterceptor());
		}
	}
}