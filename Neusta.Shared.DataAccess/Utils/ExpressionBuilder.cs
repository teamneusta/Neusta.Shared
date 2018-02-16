namespace Neusta.Shared.DataAccess.Utils
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;

	public static class ExpressionBuilder
	{
		[PublicAPI]
		public static Expression<Func<TEntity, bool>> BuildKeyEqualsExpression<TEntity, TKey>(TKey key)
			where TEntity : IID<TKey>
			where TKey : struct, IEquatable<TKey>
		{
			ParameterExpression matchParameterExpr = Expression.Parameter(typeof(TEntity), "match");
			PropertyInfo propInfo = typeof(TEntity).GetRuntimeProperty(nameof(IID.ID));
			MemberExpression idMemberAccessExpr = Expression.MakeMemberAccess(matchParameterExpr, propInfo);
			ConstantExpression keyConstantExpr = Expression.Constant(key, typeof(TKey));
			BinaryExpression compareExpr = Expression.MakeBinary(ExpressionType.Equal, idMemberAccessExpr, keyConstantExpr);
			Expression<Func<TEntity, bool>> lambdaExpr = Expression.Lambda<Func<TEntity, bool>>(compareExpr, matchParameterExpr);
			return lambdaExpr;
		}

		[PublicAPI]
		public static Expression<Func<TEntity, bool>> BuildKeyEqualsExpression<TEntity, TKey>(TKey key, string propName)
		{
			ParameterExpression matchParameterExpr = Expression.Parameter(typeof(TEntity), "match");
			PropertyInfo propInfo = typeof(TEntity).GetRuntimeProperty(propName);
			MemberExpression idMemberAccessExpr = Expression.MakeMemberAccess(matchParameterExpr, propInfo);
			ConstantExpression keyConstantExpr = Expression.Constant(key, typeof(TKey));
			BinaryExpression compareExpr = Expression.MakeBinary(ExpressionType.Equal, idMemberAccessExpr, keyConstantExpr);
			Expression<Func<TEntity, bool>> lambdaExpr = Expression.Lambda<Func<TEntity, bool>>(compareExpr, matchParameterExpr);
			return lambdaExpr;
		}

		[PublicAPI]
		public static Func<TKey, bool> BuildIsKeyEqualToDefaultFunc<TKey>()
		{
			ParameterExpression matchParameterExpr = Expression.Parameter(typeof(TKey), "match");
			ConstantExpression keyConstantExpr = Expression.Constant(default(TKey), typeof(TKey));
			BinaryExpression compareExpr = Expression.MakeBinary(ExpressionType.Equal, matchParameterExpr, keyConstantExpr);
			Expression<Func<TKey, bool>> lambdaExpr = Expression.Lambda<Func<TKey, bool>>(compareExpr, matchParameterExpr);
			return lambdaExpr.Compile();
		}

		[PublicAPI]
		public static Func<TKey, bool> BuildIsKeyGreaterThanDefaultFunc<TKey>()
		{
			ParameterExpression matchParameterExpr = Expression.Parameter(typeof(TKey), "match");
			ConstantExpression keyConstantExpr = Expression.Constant(default(TKey), typeof(TKey));
			BinaryExpression compareExpr = Expression.MakeBinary(ExpressionType.GreaterThan, matchParameterExpr, keyConstantExpr);
			Expression<Func<TKey, bool>> lambdaExpr = Expression.Lambda<Func<TKey, bool>>(compareExpr, matchParameterExpr);
			return lambdaExpr.Compile();
		}
	}
}