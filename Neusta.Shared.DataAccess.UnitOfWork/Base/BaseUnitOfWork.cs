namespace Neusta.Shared.DataAccess.UnitOfWork
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.DataAccess.Repository;
	using Neusta.Shared.DataAccess.UnitOfWork.Factory;
	using Neusta.Shared.ObjectProvider;

	public abstract class BaseUnitOfWork : DisposableObject, IUnitOfWork
	{
		private static long uniqueIDCounter;

		private SpinLock spinLockObj = new SpinLock();
		private readonly long uniqueID;
		private readonly IServiceProvider serviceProvider;
		private readonly SemaphoreSlim useLockObj = new SemaphoreSlim(1, 1);
		private Dictionary<RuntimeTypeHandle, IDataContext> contextMap;
		private Dictionary<RuntimeTypeHandle, IDataRepository> repositoryMap;
		private Dictionary<string, object> properties;
		private IUnitOfWork parentUnitOfWork;
		private IUnitOfWorkServiceFactory factory;
		private IObjectProvider objectProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseUnitOfWork"/> class.
		/// </summary>
		protected BaseUnitOfWork()
		{
			this.uniqueID = Interlocked.Increment(ref uniqueIDCounter);
			this.factory = SimpleUnitOfWorkServiceFactory.Instance;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseUnitOfWork"/> class.
		/// </summary>
		protected BaseUnitOfWork(IServiceProvider serviceProvider, IUnitOfWork parentUnitOfWork = null)
			: this()
		{
			if (serviceProvider != null)
			{
				this.serviceProvider = serviceProvider;
				if (serviceProvider is IObjectProvider objectProvider)
				{
					this.objectProvider = objectProvider;
					this.factory = new ObjectProviderUnitOfWorkServiceFactory(objectProvider);
				}
				else
				{
					this.factory = new ServiceProviderUnitOfWorkServiceFactory(serviceProvider);
				}
			}

			bool hasProperties = false;
			this.parentUnitOfWork = parentUnitOfWork;
			if (parentUnitOfWork is BaseUnitOfWork baseUnitOfWork)
			{
				if (serviceProvider == null)
				{
					this.serviceProvider = baseUnitOfWork.ServiceProvider;
					this.factory = baseUnitOfWork.Factory;
				}
				hasProperties = baseUnitOfWork.properties != null;
			}

			if (hasProperties)
			{
				var parentProperties = this.parentUnitOfWork.Properties;
				if (parentProperties.Any())
				{
					this.properties = new Dictionary<string, object>(parentProperties.Count);
					foreach (var parentProperty in parentProperties)
					{
						this.properties.Add(parentProperty.Key, parentProperty.Value);
					}
				}
			}
		}

		/// <summary>
		/// Gets the unique identifier.
		/// </summary>
		public long UniqueID
		{
			[DebuggerStepThrough]
			get { return this.uniqueID; }
		}

		/// <summary>
		/// Gets the parent unit of work.
		/// </summary>
		public IUnitOfWork ParentUnitOfWork
		{
			[DebuggerStepThrough]
			get { return this.parentUnitOfWork; }
		}

		#region Implementation of IObjectProviderProvider

		/// <summary>
		/// Gets the object provider.
		/// </summary>
		public IObjectProvider ObjectProvider
		{
			[DebuggerStepThrough]
			get { return this.objectProvider; }
		}

		#endregion

		#region Implementation of IUnitOfWork

		/// <summary>
		/// Gets the service provider.
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			[DebuggerStepThrough]
			get { return this.serviceProvider; }
		}

		/// <summary>
		/// Gets the service factory.
		/// </summary>
		public IUnitOfWorkServiceFactory Factory
		{
			[DebuggerStepThrough]
			get { return this.factory; }
		}

		/// <summary>
		/// Gets the data contexts.
		/// </summary>
		public IEnumerable<IDataContext> DataContexts
		{
			[DebuggerStepThrough]
			get { return this.contextMap?.Values ?? Enumerable.Empty<IDataContext>(); }
		}

		/// <summary>
		/// Gets the repositories.
		/// </summary>
		public IEnumerable<IDataRepository> Repositories
		{
			[DebuggerStepThrough]
			get { return this.repositoryMap?.Values ?? Enumerable.Empty<IDataRepository>(); }
		}

		/// <summary>
		/// Gets the properties.
		/// </summary>
		public IReadOnlyDictionary<string, object> Properties
		{
			get
			{
				bool lockTaken = false;
				try
				{
					if (!this.spinLockObj.IsHeldByCurrentThread)
					{
						this.spinLockObj.Enter(ref lockTaken);
					}
					Dictionary<string, object> props = this.properties;
					if (props == null)
					{
						props = new Dictionary<string, object>();
					}
					return new ReadOnlyDictionary<string, object>(props);
				}
				finally
				{
					if (lockTaken)
					{
						this.spinLockObj.Exit();
					}
				}
			}
		}

		/// <summary>
		/// Acquires a lock and returns an <see cref="IDisposable"/> that should be disposed to release the lock.
		/// </summary>
		public IDisposable AcquireLock()
		{
			this.Lock();
			return new AnonymousDisposable(this.Release);
		}

		/// <summary>
		/// Acquires a lock and returns an <see cref="IDisposable"/> that should be disposed to release the lock.
		/// </summary>
		public Task<IDisposable> AcquireLockAsync()
		{
			return this.LockAsync().ContinueWith(task => (IDisposable)new AnonymousDisposable(this.Release));
		}

		/// <summary>
		/// Tries to lock this unit of work.
		/// </summary>
		public bool TryLock()
		{
			return this.useLockObj.Wait(TimeSpan.Zero);
		}

		/// <summary>
		/// Tries to lock this unit of work within the specified amount of time.
		/// </summary>
		public bool TryLock(TimeSpan timeout)
		{
			return this.useLockObj.Wait(timeout);
		}

		/// <summary>
		/// Tries to lock this unit of work within the specified amount of time.
		/// </summary>
		public Task<bool> TryLockAsync(TimeSpan timeout)
		{
			return this.useLockObj.WaitAsync(timeout);
		}

		/// <summary>
		/// Locks this unit of work.
		/// </summary>
		public void Lock()
		{
			this.useLockObj.Wait();
		}

		/// <summary>
		/// Locks this unit of work.
		/// </summary>
		public Task LockAsync()
		{
			return this.useLockObj.WaitAsync();
		}

		/// <summary>
		/// Releases the lock.
		/// </summary>
		public void Release()
		{
			this.useLockObj.Release();
		}

		/// <summary>
		/// Gets the data context.
		/// </summary>
		public IDataContext GetDataContext(Type dataContextType)
		{
			TypeInfo typeInfo = dataContextType.GetTypeInfo();
			if (typeInfo.IsInterface || typeInfo.IsAbstract)
			{
				dataContextType = this.ResolveDataContextType(dataContextType);
			}

			IDataContext dataContext;
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				if (this.contextMap == null)
				{
					this.contextMap = new Dictionary<RuntimeTypeHandle, IDataContext>();
				}
				RuntimeTypeHandle typeHandle = dataContextType.TypeHandle;
				if (this.contextMap.TryGetValue(typeHandle, out dataContext))
				{
					if (dataContext.IsDisposed)
					{
						this.contextMap.Remove(typeHandle);
						dataContext = null;
					}
				}
				if (dataContext == null)
				{
					dataContext = this.CreateDataContext(dataContextType);
					this.contextMap.Add(typeHandle, dataContext);
				}
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}

			return dataContext;
		}

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		public TDataContext GetDataContext<TDataContext>()
			where TDataContext : IDataContext
		{
			IDataContext dataContext = this.GetDataContext(typeof(TDataContext));
			if (dataContext != null)
			{
				return (TDataContext)dataContext;
			}
			return default(TDataContext);
		}

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		public void GetDataContext<TDataContext>(out TDataContext dataContext)
			where TDataContext : IDataContext
		{
			dataContext = this.GetDataContext<TDataContext>();
		}

		/// <summary>
		/// Gets the data context.
		/// </summary>
		public IDataRepository GetRepository(Type dataRepositoryType)
		{
			TypeInfo typeInfo = dataRepositoryType.GetTypeInfo();
			if (typeInfo.IsInterface || typeInfo.IsAbstract)
			{
				dataRepositoryType = this.ResolveDataRepositoryType(dataRepositoryType);
			}

			IDataRepository dataRepository;
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				if (this.repositoryMap == null)
				{
					this.repositoryMap = new Dictionary<RuntimeTypeHandle, IDataRepository>();
				}
				RuntimeTypeHandle typeHandle = dataRepositoryType.TypeHandle;
				if (!this.repositoryMap.TryGetValue(typeHandle, out dataRepository))
				{
					dataRepository = this.CreateDataRepository(dataRepositoryType);
					this.repositoryMap.Add(typeHandle, dataRepository);
				}
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}

			return dataRepository;
		}

		/// <summary>
		/// Gets the data repository of the specified type.
		/// </summary>
		public TDataRepository GetRepository<TDataRepository>()
			where TDataRepository : IDataRepository
		{
			IDataRepository dataRepository = this.GetRepository(typeof(TDataRepository));
			if (dataRepository != null)
			{
				return (TDataRepository)dataRepository;
			}
			return default(TDataRepository);
		}

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		public void GetRepository<TDataRepository>(out TDataRepository dataRepository)
			where TDataRepository : IDataRepository
		{
			dataRepository = this.GetRepository<TDataRepository>();
		}

		/// <summary>
		/// Gets the entity repository for the specified entity type.
		/// </summary>
		public IEntityRepository<TDataContext, TEntity, TKey> GetEntityRepository<TDataContext, TEntity, TKey>()
			where TDataContext : IDataContext
			where TEntity : class, IEntityWithID<TKey>
			where TKey : struct, IEquatable<TKey>
		{
			Type type = this.ResolveEntityRepositoryType(typeof(TDataContext), typeof(TEntity), typeof(TKey));
			return this.GetRepository(type) as IEntityRepository<TDataContext, TEntity, TKey>;
		}

		/// <summary>
		/// Gets the entity repository for the specified entity type.
		/// </summary>
		public IEntityRepository<TDataContext, TEntity> GetEntityRepository<TDataContext, TEntity>()
			where TDataContext : IDataContext
			where TEntity : class, IEntity
		{
			Type type = this.ResolveEntityRepositoryType(typeof(TDataContext), typeof(TEntity));
			return this.GetRepository(type) as IEntityRepository<TDataContext, TEntity>;
		}

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		public object GetProperty(string key)
		{
			if (!this.TryGetProperty(key, out object value))
			{
				throw new KeyNotFoundException();
			}
			return value;
		}
		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		public object GetProperty(string key, object @default)
		{
			if (!this.TryGetProperty(key, out object value))
			{
				return @default;
			}
			return value;
		}

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		public TValue GetProperty<TValue>(string key)
		{
			if (!this.TryGetProperty(key, out TValue value))
			{
				throw new KeyNotFoundException();
			}
			return value;
		}

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		public TValue GetProperty<TValue>(string key, TValue @default = default(TValue))
		{
			if (!this.TryGetProperty(key, out TValue value))
			{
				return @default;
			}
			return value;
		}

		/// <summary>
		/// Sets the property value for the specified property key.
		/// </summary>
		public void SetProperty<TValue>(string key, TValue value)
		{
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				if (this.properties == null)
				{
					this.properties = new Dictionary<string, object>();
				}
				this.properties[key] = value;
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}
		}

		/// <summary>
		/// Removes the property value for the specified property key.
		/// </summary>
		public void RemoveProperty(string key)
		{
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				this.properties?.Remove(key);
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}
		}

		/// <summary>
		/// Tries the get property for the specified key, returns the optional default value if the
		/// key is not available.
		/// </summary>
		public bool TryGetProperty<TValue>(string key, out TValue value)
		{
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				if ((this.properties != null) && this.properties.TryGetValue(key, out object untypedValue))
				{
					if (untypedValue != null)
					{
						value = (TValue)untypedValue;
					}
					else
					{
						value = default(TValue);
					}
					return true;
				}
				else
				{
					value = default(TValue);
					return false;
				}
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}
		}

		/// <summary>
		/// Saves pending changes to all registered contexts.
		/// </summary>
		public void SaveChanges()
		{
			if (this.contextMap != null)
			{
				Type lastContextType = null;
				try
				{
					foreach (IDataContext context in this.contextMap.Values)
					{
						lastContextType = context.GetType();
						context.SaveChanges();
					}
				}
				catch (Exception ex)
				{
					var unitOfWorkException = new UnitOfWorkException(string.Format("UnitOfWork in context '{0}' failed to commit.", lastContextType?.Name), ex);
					throw unitOfWorkException;
				}
			}
		}

		/// <summary>
		/// Saves pending changes to all registered contexts.
		/// </summary>
		public async Task SaveChangesAsync()
		{
			if (this.contextMap != null)
			{
				IDataContext[] contextArray = this.contextMap.Values.ToArray();
				int cnt = contextArray.Length;
				var taskArray = new Task[cnt];
				try
				{
					for (var idx = 0; idx < cnt; idx++)
					{
						taskArray[idx] = contextArray[idx].SaveChangesAsync();
					}
					await Task.WhenAll(taskArray).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Type lastContextType = null;
					for (var idx = 0; idx < cnt; idx++)
					{
						if (taskArray[idx].IsFaulted)
						{
							lastContextType = contextArray[idx].GetType();
							break;
						}
					}
					var unitOfWorkException = new UnitOfWorkException(string.Format("UnitOfWork in context '{0}' failed to commit.", lastContextType?.Name), ex);
					throw unitOfWorkException;
				}
			}
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public abstract IUnitOfWork Clone();

		#endregion

		#region Explicit Implementation of IUnitOfWorkProvider

		/// <summary>
		/// Gets the current unit of work.
		/// </summary>
		IUnitOfWork IUnitOfWorkOwner.UnitOfWork
		{
			[DebuggerStepThrough]
			get { return this; }
		}

		#endregion

		#region Protected Properties and Methods

		/// <summary>
		/// Resolves the type of a data context.
		/// </summary>
		public virtual Type ResolveDataContextType(Type type)
		{
			return type;
		}

		/// <summary>
		/// Requests a data context from factory.
		/// </summary>
		protected virtual IDataContext CreateDataContext(Type dataContextType)
		{
			return this.Factory.CreateDataContext(this, dataContextType);
		}

		/// <summary>
		/// Flushes the data contexts.
		/// </summary>
		protected virtual void FlushDataContexts()
		{
			IDataContext[] items;

			// Read the list of known contexts
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				items = this.contextMap?.Values.ToArray();
				this.contextMap?.Clear();
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}

			// Dispose contexts outside of the lock
			if (items != null)
			{
				foreach (IDataContext context in items)
				{
					context.Dispose();
				}
			}
		}

		/// <summary>
		/// Resolves the type of the data repository.
		/// </summary>
		public virtual Type ResolveDataRepositoryType(Type type)
		{
			return type;
		}

		/// <summary>
		/// Resolves the type of the entity repository.
		/// </summary>
		public virtual Type ResolveEntityRepositoryType(Type dataContextType, Type entityType, Type keyType = null)
		{
			Type type;
			if (keyType == null)
			{
				type = typeof(EntityRepository<,>).MakeGenericType(dataContextType, entityType);
			}
			else
			{
				type = typeof(EntityRepository<,,>).MakeGenericType(dataContextType, entityType, keyType);
			}
			return type;
		}

		/// <summary>
		/// Requests a repository from factory.
		/// </summary>
		protected virtual IDataRepository CreateDataRepository(Type dataRepositoryType)
		{
			return this.Factory.CreateDataRepository(this, dataRepositoryType);
		}

		/// <summary>
		/// Flushes the repositories.
		/// </summary>
		protected virtual void FlushRepositories()
		{
			bool lockTaken = false;
			try
			{
				if (!this.spinLockObj.IsHeldByCurrentThread)
				{
					this.spinLockObj.Enter(ref lockTaken);
				}
				this.repositoryMap?.Clear();
			}
			finally
			{
				if (lockTaken)
				{
					this.spinLockObj.Exit();
				}
			}
		}

		#endregion

		#region Overrides of DisposableObject

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// Only need to flush data contexts (which are Disposable)
			this.FlushDataContexts();
		}

		#endregion

		#region Overrides of Object

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return $"{this.GetType().Name}[#{this.uniqueID}]";
		}

		#endregion
	}
}