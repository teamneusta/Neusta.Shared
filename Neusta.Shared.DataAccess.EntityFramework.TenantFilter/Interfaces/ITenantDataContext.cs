namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using JetBrains.Annotations;

	public interface ITenantDataContext : IDataContext, ITenantIDProvider, IInitializeTenantFilter
	{
		/// <summary>
		/// Enables the tenant filter.
		/// </summary>
		[PublicAPI]
		void EnableTenantFilter();

		/// <summary>
		/// Disables the tenant filter.
		/// </summary>
		[PublicAPI]
		void DisableTenantFilter();
	}
}