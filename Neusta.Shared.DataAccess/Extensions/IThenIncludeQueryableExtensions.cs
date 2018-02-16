// ReSharper disable once CheckNamespace

namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	public static class IThenIncludeQueryableExtensions
	{
		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		public static IThenIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
			this IThenIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
			Expression<Func<TPreviousProperty, TProperty>> expr)
			where TEntity : class
		{
			return source.DataContext.Extensions.ThenInclude(source, expr);
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		public static IThenIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
			this IThenIncludeQueryable<TEntity, TPreviousProperty> source,
			Expression<Func<TPreviousProperty, TProperty>> expr)
			where TEntity : class
		{
			return source.DataContext.Extensions.ThenInclude(source, expr);
		}
	}
}