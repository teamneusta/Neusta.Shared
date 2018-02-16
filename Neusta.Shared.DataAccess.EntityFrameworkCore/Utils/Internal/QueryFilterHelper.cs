namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Utils.Internal
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Microsoft.EntityFrameworkCore;

	internal static class QueryFilterHelper
	{
		/// <summary>
		/// Fetches entities from the repository using the specified filter, order and includes.
		/// </summary>
		public static IQueryable<TEntity> Query<TEntity>(
			IQueryable<TEntity> query,
			Expression<Func<TEntity, bool>> filter,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			string includeProperties)
			where TEntity : class
		{
			// Apply where expression filter
			if (filter != null)
			{
				query = query.Where(filter);
			}

			// Apply includes
			if (!string.IsNullOrEmpty(includeProperties))
			{
				string[] includePropertyEntries = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string includeProperty in includePropertyEntries)
				{
					query = query.Include(includeProperty);
				}
			}

			// Apply order by expression
			if (orderBy != null)
			{
				query = orderBy(query);
			}

			return query;
		}
	}
}