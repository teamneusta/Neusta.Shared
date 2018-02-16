namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;
	using Neusta.Shared.DataAccess.Entities;

	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	[Table("Tenant")]
	public class TenantEntity : BaseEntityWithID, IEntityWithID
	{
		/// <summary>
		/// Gets or sets the tenant name.
		/// </summary>
		public virtual string Name { get; set; }
	}
}