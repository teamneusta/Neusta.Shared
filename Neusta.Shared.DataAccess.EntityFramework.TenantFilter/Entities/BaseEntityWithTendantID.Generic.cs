// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;
	using Neusta.Shared.DataAccess.Entities;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils;

	[TenantFilter(TenantFilterConstants.ColumnName)]
	public abstract class BaseEntityWithTendantID<TID, TTenantEntity> : BaseEntityWithID<TID>, ITenantAwareEntity<TID, TTenantEntity>
		where TID : struct, IEquatable<TID>
	{
		/// <summary>
		/// Gets or sets the tenant identifier.
		/// </summary>
		[Index]
		[Column(TenantFilterConstants.ColumnName)]
		public virtual TID TenantID { get; set; }

		/// <summary>
		/// Gets or sets the tenant.
		/// </summary>
		[ForeignKey(nameof(TenantID))]
		public virtual TTenantEntity Tenant { get; set; }
	}
}