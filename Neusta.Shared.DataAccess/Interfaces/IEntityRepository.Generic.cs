namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;

	public interface IEntityRepository<TDataContext, TEntity> : IDataRepository<TDataContext>, IEntityRepository
		where TDataContext : IDataContext
		where TEntity : class, IEntity
	{
		/// <summary>
		/// Gets the type of the entity that this repository returns.
		/// </summary>
		[PublicAPI]
		Type EntityType { get; }

		/// <summary>
		/// Creates a new instance of the given entity class.
		/// </summary>
		[PublicAPI]
		TEntity Create();

		/// <summary>
		/// Queries all entities from the repository.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Query();

		/// <summary>
		/// Queries all entities in the given range from the repository.
		/// </summary>
		[PublicAPI]
		IEnumerable<TEntity> QueryRange(int offset, int count);

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		[PublicAPI]
		TEntity Load(params object[] keyValues);
	}
}