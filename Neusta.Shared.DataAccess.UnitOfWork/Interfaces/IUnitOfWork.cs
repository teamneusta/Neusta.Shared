// ReSharper disable once CheckNamespace
namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider;

	public interface IUnitOfWork : IDisposable, IUnitOfWorkOwner, IObjectProviderProvider, IDataContextProvider, IRepositoryProvider
	{
		/// <summary>
		/// Gets the service provider.
		/// </summary>
		[PublicAPI]
		IServiceProvider ServiceProvider { get; }

		/// <summary>
		/// Gets the service factory.
		/// </summary>
		[PublicAPI]
		IUnitOfWorkServiceFactory Factory { get; }

		/// <summary>
		/// Gets the data contexts.
		/// </summary>
		[PublicAPI]
		IEnumerable<IDataContext> DataContexts { get; }

		/// <summary>
		/// Gets the repositories.
		/// </summary>
		[PublicAPI]
		IEnumerable<IDataRepository> Repositories { get; }

		/// <summary>
		/// Gets the properties.
		/// </summary>
		[PublicAPI]
		IReadOnlyDictionary<string, object> Properties { get; }

		/// <summary>
		/// Acquires a lock and returns an <see cref="IDisposable"/> that should be disposed to release the lock.
		/// </summary>
		[PublicAPI]
		IDisposable AcquireLock();

		/// <summary>
		/// Acquires a lock and returns an <see cref="IDisposable"/> that should be disposed to release the lock.
		/// </summary>
		[PublicAPI]
		Task<IDisposable> AcquireLockAsync();

		/// <summary>
		/// Tries to lock this unit of work.
		/// </summary>
		[PublicAPI]
		bool TryLock();

		/// <summary>
		/// Tries to lock this unit of work within the specified amount of time.
		/// </summary>
		[PublicAPI]
		bool TryLock(TimeSpan timeout);

		/// <summary>
		/// Tries to lock this unit of work within the specified amount of time.
		/// </summary>
		[PublicAPI]
		Task<bool> TryLockAsync(TimeSpan timeout);

		/// <summary>
		/// Locks this unit of work.
		/// </summary>
		[PublicAPI]
		void Lock();

		/// <summary>
		/// Locks this unit of work.
		/// </summary>
		[PublicAPI]
		Task LockAsync();

		/// <summary>
		/// Releases the lock.
		/// </summary>
		[PublicAPI]
		void Release();

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		[PublicAPI]
		void GetDataContext<TDataContext>(out TDataContext dataContext)
			where TDataContext : IDataContext;

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		[PublicAPI]
		void GetRepository<TDataRepository>(out TDataRepository dataRepository)
			where TDataRepository : IDataRepository;

		/// <summary>
		/// Gets the entity repository for the specified entity type.
		/// </summary>
		[PublicAPI]
		IEntityRepository<TDataContext, TEntity, TKey> GetEntityRepository<TDataContext, TEntity, TKey>()
			where TDataContext : IDataContext
			where TEntity : class, IEntityWithID<TKey>
			where TKey : struct, IEquatable<TKey>;

		/// <summary>
		/// Gets the entity repository for the specified entity type.
		/// </summary>
		[PublicAPI]
		IEntityRepository<TDataContext, TEntity> GetEntityRepository<TDataContext, TEntity>()
			where TDataContext : IDataContext
			where TEntity : class, IEntity;

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		object GetProperty(string key);

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		object GetProperty(string key, object @default);

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		TValue GetProperty<TValue>(string key);

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		TValue GetProperty<TValue>(string key, TValue @default);

		/// <summary>
		/// Sets the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		void SetProperty<TValue>(string key, TValue value);

		/// <summary>
		/// Removes the property value for the specified property key.
		/// </summary>
		[PublicAPI]
		void RemoveProperty(string key);

		/// <summary>
		/// Tries the get property for the specified key, returns the optional default value if the
		/// key is not available.
		/// </summary>
		[PublicAPI]
		bool TryGetProperty<TValue>(string key, out TValue value);

		/// <summary>
		/// Saves pending changes to all registered contexts.
		/// </summary>
		[PublicAPI]
		void SaveChanges();

		/// <summary>
		/// Saves pending changes to all registered contexts.
		/// </summary>
		[PublicAPI]
		Task SaveChangesAsync();

		/// <summary>
		/// Clones this instance.
		/// </summary>
		[PublicAPI]
		IUnitOfWork Clone();
	}
}