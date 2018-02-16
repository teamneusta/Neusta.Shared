namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Configuration
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Configuration;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Internals;

	[PublicAPI]
	public abstract class TenantDbConfiguration : OptimizedDbConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TenantDbConfiguration"/> class.
		/// </summary>
		protected TenantDbConfiguration()
		{
			this.AddInterceptor(new TenantCommandInterceptor());
			this.AddInterceptor(new TenantCommandTreeInterceptor());
		}
	}
}