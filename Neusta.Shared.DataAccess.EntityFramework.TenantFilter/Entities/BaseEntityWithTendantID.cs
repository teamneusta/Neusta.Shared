namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities
{
	using System;

	public abstract class BaseEntityWithTendantID : BaseEntityWithTendantID<long, TenantEntity>, ITenantAwareEntity
	{
	}
}