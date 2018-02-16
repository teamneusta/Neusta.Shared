using System;

namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Internals
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Core.Common.CommandTrees;
	using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure.Interception;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	/// <summary>
	/// Custom implementation of <see cref="IDbCommandTreeInterceptor"/> which filters based on tenantId.
	/// </summary>
	internal class TenantCommandTreeInterceptor : IDbCommandTreeInterceptor
	{
		/// <summary>
		/// This method is called after a new <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" /> has been created.
		/// The tree that is used after interception can be changed by setting
		/// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Result" /> while intercepting.
		/// </summary>
		/// <remarks>
		/// Command trees are created for both queries and insert/update/delete commands. However, query
		/// command trees are cached by model which means that command tree creation only happens the
		/// first time a query is executed and this notification will only happen at that time
		/// </remarks>
		public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
		{
			if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
			{
				return;
			}

			// No automatic tenant filter if it is not enabled in data context
			ITenantIDProvider tenantIDProvider = interceptionContext.DbContexts.OfType<ITenantIDProvider>().FirstOrDefault();
			if (tenantIDProvider == null || !tenantIDProvider.TenantFilterEnabled)
			{
				return;
			}
			var tenantDbContext = tenantIDProvider as DbContext;

			// In case of query command change the query by adding a filtering based on tenantId
			var queryCommand = interceptionContext.Result as DbQueryCommandTree;
			if (queryCommand != null)
			{
				DbExpression newQuery = queryCommand.Query.Accept(new TenantQueryVisitor());
				interceptionContext.Result = new DbQueryCommandTree(
					queryCommand.MetadataWorkspace,
					queryCommand.DataSpace,
					newQuery);
				return;
			}

			// Check for a modification statement
			var modificationCommand = interceptionContext.Result as DbModificationCommandTree;
			if (modificationCommand == null)
			{
				return;
			}

			// Get tenant ID
			long? tenantID = tenantIDProvider.TenantID;
			if (tenantID.HasValue)
			{
				// Make sure that we cannot update or delete records from other tenants
				// Do not handle inserts, because they need the Tenant navigation property which cannot be set here
				long tenantIDValue = tenantID.Value;
				if (InterceptUpdate(interceptionContext, tenantDbContext, tenantIDValue))
				{
					return;
				}
				InterceptDeleteCommand(interceptionContext, tenantDbContext, tenantIDValue);
			}
		}

		/// <summary>
		/// In case of an update command we always filter based on the tenantId
		/// </summary>
		[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
		private static bool InterceptUpdate(DbCommandTreeInterceptionContext interceptionContext, DbContext context, long tenantID)
		{
			var updateCommand = interceptionContext.Result as DbUpdateCommandTree;
			if (updateCommand == null)
			{
				return false;
			}

			EdmType edmType = updateCommand.Target.VariableType.EdmType;
			string columnName = TenantFilterAttribute.GetTenantColumnName(edmType);
			if (string.IsNullOrEmpty(columnName))
			{
				return false;
			}

			// Create the variable reference in order to create the property
			DbVariableReferenceExpression variableReference = DbExpressionBuilder.Variable(updateCommand.Target.VariableType,
				updateCommand.Target.VariableName);
			// Create the property to which will assign the correct value
			DbPropertyExpression tenantProperty = DbExpressionBuilder.Property(variableReference, columnName);
			// Create the tenantId where predicate, object representation of sql where tenantId = value statement
			DbComparisonExpression tenantIdWherePredicate = DbExpressionBuilder.Equal(tenantProperty, DbExpression.FromInt64(tenantID));

			// The initial predicate is the sql where statement
			DbExpression initialPredicate = updateCommand.Predicate;
			// Add to the initial statement the tenantId statement which translates in sql AND TenantId = 'value'
			DbAndExpression finalPredicate = initialPredicate.And(tenantIdWherePredicate);

			var newUpdateCommand = new DbUpdateCommandTree(
				updateCommand.MetadataWorkspace,
				updateCommand.DataSpace,
				updateCommand.Target,
				finalPredicate,
				updateCommand.SetClauses.ToReadOnly(),
				updateCommand.Returning);

			interceptionContext.Result = newUpdateCommand;

			// True means an interception successfully happened so there is no need to continue
			return true;
		}

		/// <summary>
		/// In case of a delete command we always filter based on the tenantId
		/// </summary>
		[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
		private static void InterceptDeleteCommand(DbCommandTreeInterceptionContext interceptionContext, DbContext context, long tenantID)
		{
			var deleteCommand = interceptionContext.Result as DbDeleteCommandTree;
			if (deleteCommand == null)
			{
				return;
			}

			EdmType edmType = deleteCommand.Target.VariableType.EdmType;
			string columnName = TenantFilterAttribute.GetTenantColumnName(edmType);
			if (string.IsNullOrEmpty(columnName))
			{
				return;
			}

			// Create the variable reference in order to create the property
			DbVariableReferenceExpression variableReference = DbExpressionBuilder.Variable(deleteCommand.Target.VariableType,
				deleteCommand.Target.VariableName);
			// Create the property to which will assign the correct value
			DbPropertyExpression tenantProperty = DbExpressionBuilder.Property(variableReference, columnName);
			DbComparisonExpression tenantIdWherePredicate = DbExpressionBuilder.Equal(tenantProperty, DbExpression.FromInt64(tenantID));

			// The initial predicate is the sql where statement
			DbExpression initialPredicate = deleteCommand.Predicate;
			// Add to the initial statement the tenantId statement which translates in sql AND TenantId = 'value'
			DbAndExpression finalPredicate = initialPredicate.And(tenantIdWherePredicate);

			var newDeleteCommand = new DbDeleteCommandTree(
				deleteCommand.MetadataWorkspace,
				deleteCommand.DataSpace,
				deleteCommand.Target,
				finalPredicate);

			interceptionContext.Result = newDeleteCommand;
		}
	}
}