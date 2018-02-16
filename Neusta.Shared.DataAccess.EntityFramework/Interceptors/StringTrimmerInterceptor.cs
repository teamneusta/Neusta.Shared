namespace Neusta.Shared.DataAccess.EntityFramework.Interceptors
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Core.Common.CommandTrees;
	using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure.Interception;
	using System.Linq;

	public class StringTrimmerInterceptor : IDbCommandTreeInterceptor
	{
		/// <summary>
		/// This method is called after a new <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" /> has been created.
		/// The tree that is used after interception can be changed by setting
		/// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Result" /> while intercepting.
		/// </summary>
		public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
		{
			if (interceptionContext.OriginalResult.DataSpace == DataSpace.SSpace)
			{
				var queryCommand = interceptionContext.Result as DbQueryCommandTree;
				if (queryCommand != null)
				{
					DbExpression newQuery = queryCommand.Query.Accept(new StringTrimmerQueryVisitor());
					interceptionContext.Result = new DbQueryCommandTree(
						queryCommand.MetadataWorkspace,
						queryCommand.DataSpace,
						newQuery);
				}
			}
		}

		private class StringTrimmerQueryVisitor : DefaultExpressionVisitor
		{
			private static readonly string[] typesToTrim = { "nvarchar", "varchar", "char", "nchar" };

			/// <summary>
			/// Implements the visitor pattern for the construction of a new instance of a given type, including set and record types.
			/// </summary>
			public override DbExpression Visit(DbNewInstanceExpression expression)
			{
				IEnumerable<DbExpression> arguments = expression.Arguments.Select(a =>
				{
					var propertyArg = a as DbPropertyExpression;
					if (propertyArg != null && typesToTrim.Contains(propertyArg.Property.TypeUsage.EdmType.Name))
					{
						return a.TrimEnd();
					}
					return a;
				});
				return expression.ResultType.New(arguments);
			}
		}
	}
}