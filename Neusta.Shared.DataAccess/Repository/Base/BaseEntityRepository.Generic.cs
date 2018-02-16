namespace Neusta.Shared.DataAccess.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Neusta.Shared.DataAccess.Utils;

	public abstract class BaseEntityRepository<TDataContext, TEntity> : BaseEntityRepository, IEntityRepository<TDataContext, TEntity>
		where TDataContext : IDataContext
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseEntityRepository{TDataContext,TEntity}"/> class.
		/// </summary>
		protected BaseEntityRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}

		#region Implementation of IDataContextOwner<TDataContext>

		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		public new TDataContext DataContext
		{
			[DebuggerStepThrough]
			get { return (TDataContext)base.DataContext; }
		}

		#endregion

		#region Implementation of IEntityRepository<TDataContext,TEntity>

		/// <summary>
		/// Gets the type of the entity that this repository returns.
		/// </summary>
		public Type EntityType
		{
			[DebuggerStepThrough]
			get { return typeof(TEntity); }
		}

		/// <summary>
		/// Creates a new instance of the given entity class.
		/// </summary>
		public TEntity Create()
		{
			return EntityActivator<TEntity>.Create();
		}

		/// <summary>
		/// Queries all entities from the repository.
		/// </summary>
		public IQueryable<TEntity> Query()
		{
			return this.DataContext.Query<TEntity>();
		}

		/// <summary>
		/// Queries all entities in the given range from the repository.
		/// </summary>
		public IEnumerable<TEntity> QueryRange(int offset, int count)
		{
			IEnumerable<TEntity> query = this.DataContext.Query<TEntity>();
			if (offset >= 0)
			{
				query = query.Skip(offset);
			}
			if (count > 0)
			{
				query = query.Take(count);
			}
			return query;
		}

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		public TEntity Load(params object[] keyValues)
		{
			return this.DataContext.Load<TEntity>(keyValues);
		}

		#endregion
	}
}