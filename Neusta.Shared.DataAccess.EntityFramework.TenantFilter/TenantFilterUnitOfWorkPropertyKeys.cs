namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	public static class TenantFilterUnitOfWorkPropertyKeys
	{
		public static readonly string TenantFilterEnabledKey = string.Intern(@"__tnhb|TenantFilterEnabled");
		public static readonly string TenantIDKey = string.Intern(@"__tnhb|TenantID");
	}
}