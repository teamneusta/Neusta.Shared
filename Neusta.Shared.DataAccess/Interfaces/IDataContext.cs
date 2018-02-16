namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;

	public interface IDataContext : IDisposableObject, IDataContextOwner, IExecuteThreadSafe
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		[PublicAPI]
		string Name { get; }

		/// <summary>
		/// Gets the unique identifier.
		/// </summary>
		[PublicAPI]
		long UniqueID { get; }

		/// <summary>
		/// Gets a value indicating whether this data context is dirty.
		/// </summary>
		[PublicAPI]
		bool IsDirty { get; }

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		[PublicAPI]
		TEntity Load<TEntity>(params object[] keyValues)
			where TEntity : class, IEntity;

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		[PublicAPI]
		IEntity Load(Type entityType, params object[] keyValues);

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		[PublicAPI]
		TEntity Load<TEntity, TKey>(TKey id)
			where TEntity : class, IEntityWithID<TKey>
			where TKey : struct, IEquatable<TKey>;

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		[PublicAPI]
		TEntity Load<TEntity, TKey>(TKey? id)
			where TEntity : class, IEntityWithID<TKey>
			where TKey : struct, IEquatable<TKey>;

		/// <summary>
		/// Returns an <see cref="IQueryable{TEntity}"/> that allows to query the database.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Query<TEntity>()
			where TEntity : class, IEntity;

		/// <summary>
		/// Creates a new entity of the given type.
		/// </summary>
		[PublicAPI]
		TEntity Create<TEntity>()
			where TEntity : class, IEntity;

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		[PublicAPI]
		void Add<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Adds the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void AddRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Adds the given list of entities.
		/// </summary>
		[PublicAPI]
		void AddRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		[PublicAPI]
		void Update<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Updates the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Updates the given list of entities.
		/// </summary>
		[PublicAPI]
		void UpdateRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		[PublicAPI]
		void Delete<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Deletes the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void DeleteRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Deletes the given list of entities.
		/// </summary>
		[PublicAPI]
		void DeleteRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Attaches the specified entity.
		/// </summary>
		[PublicAPI]
		void Attach<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Attaches the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void AttachRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Attaches the given list of entities.
		/// </summary>
		[PublicAPI]
		void AttachRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Detaches the specified entity.
		/// </summary>
		[PublicAPI]
		void Detach<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Detaches the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void DetachRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Detaches the given list of entities.
		/// </summary>
		[PublicAPI]
		void DetachRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Refreshes the specified entity.
		/// </summary>
		[PublicAPI]
		void Refresh<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Refreshes the given list of entities of the same type.
		/// </summary>
		[PublicAPI]
		void RefreshRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Refreshes the given list of entities.
		/// </summary>
		[PublicAPI]
		void RefreshRange(IEnumerable<IEntity> entities);

		/// <summary>
		/// Detects changes to all known entities
		/// </summary>
		[PublicAPI]
		void DetectChanges();

		/// <summary>
		/// Gets the current change set.
		/// </summary>
		[PublicAPI]
		IChangeSet GetChangeSet();

		/// <summary>
		/// Saves the changes.
		/// </summary>
		[PublicAPI]
		void SaveChanges();

		/// <summary>
		/// Saves the changes as an asynchronous operation.
		/// </summary>
		[PublicAPI]
		Task SaveChangesAsync();

		/// <summary>
		/// Resets this data context.
		/// </summary>
		[PublicAPI]
		void Reset();
	}
}