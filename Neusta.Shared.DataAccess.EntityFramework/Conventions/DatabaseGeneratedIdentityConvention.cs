namespace Neusta.Shared.DataAccess.EntityFramework.Conventions
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using Neusta.Shared.Core;

	public class DatabaseGeneratedIdentityConvention : Convention
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseGeneratedIdentityConvention"/> class.
		/// </summary>
		public DatabaseGeneratedIdentityConvention()
		{
			this.ProcessProperties<int>();
			this.ProcessProperties<long>();
		}

		private void ProcessProperties<TValueType>()
		{
			this.Properties<TValueType>()
				.Where(p => p.PropertyType.IsValueType
				            && string.Equals(p.Name, "ID", StringComparison.OrdinalIgnoreCase)
				            && typeof(IID<>).MakeGenericType(p.PropertyType).IsAssignableFrom(p.DeclaringType))
				.Configure(c => c
					.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
				);
		}
	}
}