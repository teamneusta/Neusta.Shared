namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IEntityFrameworkEntityRepository<TDataContext, TEntity> : IEntityRepository<TDataContext, TEntity>
		where TDataContext : IEntityFrameworkDataContext<TDataContext>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Fetches entities from the repository using the specified filter, order and includes.
		/// </summary>
		[PublicAPI]
		IEnumerable<TEntity> Fetch(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null);

		/// <summary>
		/// Fetches entities from the repository using the specified filter, order and includes.
		/// </summary>
		[PublicAPI]
		Task<IEnumerable<TEntity>> FetchAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null);
	}
}