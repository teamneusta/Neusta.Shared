namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;
	using Neusta.Shared.DataAccess.Repository;

	[PublicAPI]
	public abstract class EntityFrameworkEntityRepository<TDataContext, TEntity> : BaseEntityRepository<TDataContext, TEntity>, IEntityFrameworkEntityRepository<TDataContext, TEntity>
		where TDataContext : IEntityFrameworkDataContext<TDataContext>
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkEntityRepository{TDataContext, TEntity}"/> class.
		/// </summary>
		protected EntityFrameworkEntityRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}

		#region Implementation of IEntityFrameworkDataRepository<TEntity>

		/// <summary>
		/// Fetches entities from the repository using the specified filter, order and includes.
		/// </summary>
		public virtual IEnumerable<TEntity> Fetch(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null)
		{
			return QueryFilterHelper.Query(this.DataContext.Query<TEntity>(), filter, orderBy, includeProperties).ToList();
		}

		/// <summary>
		/// Fetches entities from the repository using the specified filter, order and includes.
		/// </summary>
		public virtual async Task<IEnumerable<TEntity>> FetchAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null)
		{
			return await QueryFilterHelper.Query(this.DataContext.Query<TEntity>(), filter, orderBy, includeProperties).ToListAsync().ConfigureAwait(false);
		}

		#endregion
	}
}