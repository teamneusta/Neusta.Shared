namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities;

	public interface ITenantAwareEntity : ITenantAwareEntity<long, TenantEntity>, IEntityWithID
	{
	}
}