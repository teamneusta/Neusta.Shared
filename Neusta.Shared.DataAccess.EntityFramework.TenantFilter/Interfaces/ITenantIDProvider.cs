namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using JetBrains.Annotations;

	public interface ITenantIDProvider
	{
		/// <summary>
		/// Gets a value indicating whether the tenant filter is enabled.
		/// </summary>
		[PublicAPI]
		bool TenantFilterEnabled { get; }

		/// <summary>
		/// Gets the tenant identifier.
		/// </summary>
		[PublicAPI]
		long? TenantID { get; }
	}
}