using System;

namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Internals
{
	using System;
	using System.Collections.Generic;
	using System.Data.Common;
	using System.Data.Entity.Infrastructure.Interception;
	using System.Linq;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils;

	/// <summary>
	/// Custom implementation of <see cref="IDbCommandInterceptor"/>.
	/// In this class we set the actual value of the tenantId when querying the database as the command tree is cached
	/// </summary>
	internal class TenantCommandInterceptor : IDbCommandInterceptor
	{
		/// <summary>
		/// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" /> or
		/// one of its async counterparts is made.
		/// </summary>
		public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
		{
			SetTenantParameterValue(command, interceptionContext);
		}

		/// <summary>
		/// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" />  or
		/// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
		/// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
		/// </summary>
		public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
		{
		}

		/// <summary>
		/// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
		/// one of its async counterparts is made.
		/// </summary>
		public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
		{
			SetTenantParameterValue(command, interceptionContext);
		}

		/// <summary>
		/// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
		/// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
		/// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
		/// </summary>
		public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
		{
		}

		/// <summary>
		/// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" /> or
		/// one of its async counterparts is made.
		/// </summary>
		public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
		{
			SetTenantParameterValue(command, interceptionContext);
		}

		/// <summary>
		/// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" /> or
		/// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
		/// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
		/// </summary>
		public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
		{
		}

		private static void SetTenantParameterValue(DbCommand command, DbInterceptionContext interceptionContext)
		{
			// Check command
			if (command == null || command.Parameters.Count == 0)
			{
				return;
			}

			// No automatic tenant filter if it is not enabled in data context
			ITenantIDProvider tenantIDProvider = interceptionContext.DbContexts.OfType<ITenantIDProvider>().FirstOrDefault();
			if (tenantIDProvider == null || !tenantIDProvider.TenantFilterEnabled)
			{
				return;
			}

			// Get tenant ID
			long? tenantID = tenantIDProvider.TenantID;
			if (!tenantID.HasValue)
			{
				return;
			}

			// Enumerate all command parameters and assign the correct value in the one we added inside query visitor
			IEnumerable<DbParameter> dbParameters = command.Parameters.OfType<DbParameter>().Where(
				match => match.ParameterName == TenantFilterConstants.ParameterName);

			foreach (DbParameter dbParameter in dbParameters)
			{
				dbParameter.Value = tenantID.Value;
			}
		}
	}
}