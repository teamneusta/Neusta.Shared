namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using JetBrains.Annotations;

	public interface IInitializeTenantFilter
	{
		/// <summary>
		/// Initializes the tenant filter.
		/// </summary>
		[PublicAPI]
		void InitializeTenantFilter(long tenantID);
	}
}