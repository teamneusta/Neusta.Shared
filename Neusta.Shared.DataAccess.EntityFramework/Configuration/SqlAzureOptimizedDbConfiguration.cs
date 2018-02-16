using System;

namespace Neusta.Shared.DataAccess.EntityFramework.Configuration
{
	using System.Data.Entity.SqlServer;

	public abstract class SqlAzureOptimizedDbConfiguration : OptimizedDbConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlAzureOptimizedDbConfiguration"/> class.
		/// </summary>
		protected SqlAzureOptimizedDbConfiguration()
		{
			this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
		}
	}
}