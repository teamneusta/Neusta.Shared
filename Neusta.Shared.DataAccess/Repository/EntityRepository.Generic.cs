using System;

namespace Neusta.Shared.DataAccess.Repository
{
	public sealed class EntityRepository<TDataContext, TEntity> : BaseEntityRepository<TDataContext, TEntity>
		where TDataContext : IDataContext
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityRepository{TDataContext,TEntity}"/> class.
		/// </summary>
		public EntityRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}
	}
}
