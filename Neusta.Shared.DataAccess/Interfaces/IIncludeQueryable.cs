namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	public interface IIncludeQueryable<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		IDataContext DataContext { get; }

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IThenIncludeQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> expr);
	}
}