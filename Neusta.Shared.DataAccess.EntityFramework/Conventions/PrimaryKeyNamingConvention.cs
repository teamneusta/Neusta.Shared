namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using Neusta.Shared.Core;

	public class PrimaryKeyNamingConvention : Convention
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PrimaryKeyNamingConvention"/> class.
		/// </summary>
		public PrimaryKeyNamingConvention()
		{
			this.Properties()
				.Where(p => p.PropertyType.IsValueType
				            && string.Equals(p.Name, "ID", StringComparison.OrdinalIgnoreCase)
				            && typeof(IID<>).MakeGenericType(p.PropertyType).IsAssignableFrom(p.DeclaringType))
				.Configure(c => c
					.HasColumnName("PkID")
					.HasColumnOrder(0)
					.IsKey()
				);
		}
	}
}