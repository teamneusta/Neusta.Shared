// ReSharper disable InvokeAsExtensionMethod

using System;

namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Internals
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Core.Common.CommandTrees;
	using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils;

	/// <summary>
	/// Visitor pattern implementation class that adds filtering for tenantId column if applicable
	/// </summary>
	internal class TenantQueryVisitor : DefaultExpressionVisitor
	{
		private readonly HashSet<DbExpression> visitedExpressions = new HashSet<DbExpression>();

		/// <summary>
		/// This method called before the one below it when a filtering is already exists in the query (e.g. fetch an entity by id)
		/// so we apply the dynamic filtering at this level
		/// </summary>
		public override DbExpression Visit(DbFilterExpression expression)
		{
			DbExpression resultExpression = base.Visit(expression);

			string columnName = TenantFilterAttribute.GetTenantColumnName(expression.Input.Variable.ResultType.EdmType);
			if (!string.IsNullOrEmpty(columnName) && this.visitedExpressions.Add(resultExpression))
			{
				DbFilterExpression newFilterExpression = this.BuildFilterExpression(expression.Input, expression.Predicate, columnName);
				// If not null, a new DbFilterExpression has been created with our dynamic filters.
				if (newFilterExpression != null)
				{
					resultExpression = base.Visit(newFilterExpression);
				}
			}

			return resultExpression;
		}

		public override DbExpression Visit(DbScanExpression expression)
		{
			DbExpression resultExpression = base.Visit(expression);

			string columnName = TenantFilterAttribute.GetTenantColumnName(expression.Target.ElementType);
			if (!string.IsNullOrEmpty(columnName) && this.visitedExpressions.Add(resultExpression))
			{
				// Get the current expression binding
				DbExpressionBinding currentExpressionBinding = DbExpressionBuilder.Bind(resultExpression);
				DbFilterExpression newFilterExpression = this.BuildFilterExpression(currentExpressionBinding, null, columnName);

				// If not null, a new DbFilterExpression has been created with our dynamic filters.
				if (newFilterExpression != null)
				{
					resultExpression = base.Visit(newFilterExpression);
				}
			}

			return resultExpression;
		}

		/// <summary>
		/// Helper method creating the correct filter expression based on the supplied parameters
		/// </summary>
		private DbFilterExpression BuildFilterExpression(DbExpressionBinding binding, DbExpression predicate, string column)
		{
			DbVariableReferenceExpression variableReference = DbExpressionBuilder.Variable(binding.VariableType, binding.VariableName);

			// Create the property based on the variable in order to apply the equality
			DbPropertyExpression tenantProperty = DbExpressionBuilder.Property(variableReference, column);

			// Create the parameter which is an object representation of a sql parameter.
			// We have to create a parameter and not perform a direct comparison with Equal function for example
			// as this logic is cached per query and called only once
			DbParameterReferenceExpression tenantParameter = DbExpressionBuilder.Parameter(tenantProperty.Property.TypeUsage, TenantFilterConstants.ParameterName);

			// Apply the equality between property and parameter.
			DbExpression newPredicate = DbExpressionBuilder.Equal(tenantProperty, tenantParameter);

			// If an existing predicate exists (normally when called from DbFilterExpression) execute a logical AND to get the result
			if (predicate != null)
			{
				newPredicate = predicate.And(newPredicate);
			}

			return DbExpressionBuilder.Filter(binding, newPredicate);
		}
	}
}