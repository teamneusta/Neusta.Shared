namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	public interface ITenantFilterUnitOfWork : IUnitOfWork, IInitializeTenantFilter, ITenantIDProvider
	{
	}
}