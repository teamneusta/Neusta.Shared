namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils
{
	using System;
	using JetBrains.Annotations;

	public static class TenantFilterConstants
	{
		[UsedImplicitly]
		public const string AnnotationName = "TenantPropertyName";

		[UsedImplicitly]
		public const string ColumnName = "FkTenantID";

		[UsedImplicitly]
		public const string PropertyName = "TenantID";

		[UsedImplicitly]
		public const string TenantPropertyName = "Tenant";

		[UsedImplicitly]
		public const string ParameterName = "TenantIDParameter";
	}
}