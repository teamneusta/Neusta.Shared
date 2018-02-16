namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	using System;

	public interface IEntityFrameworkEntityRepository<TDataContext, TEntity, TKey> : IEntityRepository<TDataContext, TEntity, TKey>, IEntityFrameworkEntityRepository<TDataContext, TEntity>
		where TDataContext : IEntityFrameworkDataContext<TDataContext>
		where TEntity : class, IEntityWithID<TKey>
		where TKey : struct, IEquatable<TKey>
	{
	}
}