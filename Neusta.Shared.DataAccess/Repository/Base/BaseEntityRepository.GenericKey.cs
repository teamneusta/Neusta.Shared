namespace Neusta.Shared.DataAccess.Repository
{
	using System;

	public abstract class BaseEntityRepository<TDataContext, TEntity, TKey> : BaseEntityRepository<TDataContext, TEntity>, IEntityRepository<TDataContext, TEntity, TKey>
		where TDataContext : IDataContext
		where TEntity : class, IEntityWithID<TKey>
		where TKey : struct, IEquatable<TKey>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseEntityRepository{TDataContext,TEntity,TKey}"/> class.
		/// </summary>
		protected BaseEntityRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}

		#region Implementation of IDataRepository<TEntity,TKey>

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		public TEntity Load(TKey id)
		{
			return this.DataContext.Load<TEntity, TKey>(id);
		}

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		public TEntity Load(TKey? id)
		{
			return this.DataContext.Load<TEntity, TKey>(id);
		}

		#endregion
	}
}