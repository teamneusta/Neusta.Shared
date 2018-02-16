namespace Neusta.Shared.DataAccess.Repository
{
	using System;

	public sealed class EntityRepository<TDataContext, TEntity, TKey> : BaseEntityRepository<TDataContext, TEntity, TKey>
		where TDataContext : IDataContext
		where TEntity : class, IEntityWithID<TKey>
		where TKey : struct, IEquatable<TKey>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityRepository{TDataContext,TEntity,TKey}"/> class.
		/// </summary>
		public EntityRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}
	}
}