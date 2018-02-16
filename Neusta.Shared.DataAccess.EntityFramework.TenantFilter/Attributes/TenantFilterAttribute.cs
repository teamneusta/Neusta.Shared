namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Diagnostics;
	using System.Linq;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils;

	/// <summary>
	/// Attribute used to mark all entities which should be filtered based on tenantId.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class TenantFilterAttribute : Attribute
	{
		private readonly string columnName;

		/// <summary>
		/// Initializes a new instance of the <see cref="TenantFilterAttribute"/> class.
		/// </summary>
		public TenantFilterAttribute(string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
			{
				throw new ArgumentNullException(nameof(columnName));
			}
			this.columnName = columnName;
		}

		/// <summary>
		/// Gets the name of the tenant property.
		/// </summary>
		public string ColumnName
		{
			[DebuggerStepThrough]
			get { return this.columnName; }
		}

		/// <summary>
		/// Gets the name of the tenant column.
		/// </summary>
		public static string GetTenantColumnName(EdmType type)
		{
			MetadataProperty annotation = type.MetadataProperties.SingleOrDefault(
				metadataProperty => metadataProperty.Name.EndsWith($"customannotation:{TenantFilterConstants.AnnotationName}"));

			return annotation?.Value as string;
		}
	}
}