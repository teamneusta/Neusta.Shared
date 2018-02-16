using System;

namespace Neusta.Shared.DataAccess.EntityFramework.Configuration
{
	using System.Data.Entity;
	using System.Data.Entity.SqlServer;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;

	public abstract class SqlAzureDbConfiguration : DbConfiguration
	{
		/// <summary>
		/// Initializes static members of the <see cref="SqlAzureDbConfiguration"/> class.
		/// </summary>
		static SqlAzureDbConfiguration()
		{
			ZExtensionsLicenseManager.Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlAzureDbConfiguration"/> class.
		/// </summary>
		protected SqlAzureDbConfiguration()
		{
			this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
		}
	}
}