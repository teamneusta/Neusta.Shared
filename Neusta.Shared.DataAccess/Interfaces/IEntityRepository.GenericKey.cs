namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;

	public interface IEntityRepository<TDataContext, TEntity, TKey> : IEntityRepository<TDataContext, TEntity>, IEntityRepository
		where TDataContext : IDataContext
		where TEntity : class, IEntityWithID<TKey>
		where TKey : struct, IEquatable<TKey>
	{
		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		[PublicAPI]
		TEntity Load(TKey id);

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		[PublicAPI]
		TEntity Load(TKey? id);
	}
}