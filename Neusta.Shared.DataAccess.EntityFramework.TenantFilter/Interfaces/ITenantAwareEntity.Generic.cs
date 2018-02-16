namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using JetBrains.Annotations;

	public interface ITenantAwareEntity<TID, TTenantEntity> : IEntityWithID<TID>
		where TID : struct, IEquatable<TID>
	{
		/// <summary>
		/// Gets or sets the tenant identifier.
		/// </summary>
		[PublicAPI]
		TID TenantID { get; set; }

		/// <summary>
		/// Gets or sets the tenant.
		/// </summary>
		[PublicAPI]
		TTenantEntity Tenant { get; set; }
	}
}