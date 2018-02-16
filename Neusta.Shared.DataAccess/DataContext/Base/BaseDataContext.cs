namespace Neusta.Shared.DataAccess.DataContext
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.DataAccess.Changes;
	using Neusta.Shared.DataAccess.Utils;
	using Neusta.Shared.Logging;

	public abstract class BaseDataContext<TDataContext> : DisposableObject, IDataContext<TDataContext>, IDataContextExtensions
		where TDataContext : BaseDataContext<TDataContext>
	{
		// ReSharper disable StaticMemberInGenericType
		private static readonly ILogger logger = LogManager.GetLogger<TDataContext>();

		private static long uniqueIDCounter;
		// ReSharper restore StaticMemberInGenericType

		private readonly string name;
		private readonly long uniqueID;
		private readonly SemaphoreSlim syncRootSemaphore;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDataContext{TDataContext}"/> class.
		/// </summary>
		protected BaseDataContext()
		{
			this.name = this.GetType().Name;
			this.uniqueID = Interlocked.Increment(ref uniqueIDCounter);
			this.syncRootSemaphore = new SemaphoreSlim(1, 1);
		}

		/// <summary>
		/// Gets the logger.
		/// </summary>
		protected static ILogger Logger
		{
			[DebuggerStepThrough]
			get { return logger; }
		}

		#region Implementation of IDataContext

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
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
		/// Gets a value indicating whether this data context is dirty.
		/// </summary>
		public abstract bool IsDirty { get; }

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		public virtual TEntity Load<TEntity>(params object[] keyValues)
			where TEntity : class, IEntity
		{
			var entity = this.Load(typeof(TEntity), keyValues);
			if (entity == default(TEntity))
			{
				return default(TEntity);
			}
			return (TEntity)entity;
		}

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		public abstract IEntity Load(Type entityType, params object[] keyValues);

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		public virtual TEntity Load<TEntity, TID>(TID id)
			where TEntity : class, IEntityWithID<TID>
			where TID : struct, IEquatable<TID>
		{
			return this.Query<TEntity>().FirstOrDefault(match => match.Equals(id));
		}

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		public virtual TEntity Load<TEntity, TID>(TID? id)
			where TEntity : class, IEntityWithID<TID>
			where TID : struct, IEquatable<TID>
		{
			if (id.HasValue)
			{
				return this.Load<TEntity, TID>(id.Value);
			}
			return default(TEntity);
		}

		/// <summary>
		/// Returns an <see cref="T:System.Linq.IQueryable`1" /> that allows to query the database.
		/// </summary>
		public abstract IQueryable<TEntity> Query<TEntity>()
			where TEntity : class, IEntity;

		/// <summary>
		/// Creates a new entity of the given type.
		/// </summary>
		public virtual TEntity Create<TEntity>()
			where TEntity : class, IEntity
		{
			return EntityActivator<TEntity>.Create();
		}

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		public abstract void Add<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Adds the given list of entities of the same type.
		/// </summary>
		public virtual void AddRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.AddRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Adds the given list of entities of the same or different entity type.
		/// </summary>
		public virtual void AddRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Add);
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		public abstract void Update<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Updates the given list of entities of the same type.
		/// </summary>
		public virtual void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.UpdateRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Updates the given list of entities.
		/// </summary>
		public virtual void UpdateRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Update);
		}

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		public abstract void Delete<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Deletes the given list of entities of the same type.
		/// </summary>
		public virtual void DeleteRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.DeleteRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Deletes the given list of entities.
		/// </summary>
		public virtual void DeleteRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Delete);
		}

		/// <summary>
		/// Attaches the specified entity.
		/// </summary>
		public abstract void Attach<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Attaches the given list of entities of the same type.
		/// </summary>
		public virtual void AttachRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.AttachRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Attaches the given list of entities.
		/// </summary>
		public virtual void AttachRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Attach);
		}

		/// <summary>
		/// Detaches the specified entity.
		/// </summary>
		public abstract void Detach<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Detaches the given list of entities of the same type.
		/// </summary>
		public virtual void DetachRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.DetachRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Detaches the given list of entities.
		/// </summary>
		public virtual void DetachRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Detach);
		}

		/// <summary>
		/// Refreshes the specified entity.
		/// </summary>
		public abstract void Refresh<TEntity>(TEntity entity)
			where TEntity : class, IEntity;

		/// <summary>
		/// Refreshes the specified list of entities of the same type
		/// </summary>
		public virtual void RefreshRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			this.RefreshRange(entities.Cast<IEntity>());
		}

		/// <summary>
		/// Refreshes the given list of entities.
		/// </summary>
		public virtual void RefreshRange(IEnumerable<IEntity> entities)
		{
			entities.ForEach(this.Refresh);
		}

		/// <summary>
		/// Detects changes to all known entities
		/// </summary>
		public virtual void DetectChanges()
		{
		}

		/// <summary>
		/// Gets the current change set.
		/// </summary>
		public virtual IChangeSet GetChangeSet()
		{
			return ChangeSet.Empty;
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public abstract void SaveChanges();

		/// <summary>
		/// Saves the changes as an asynchronous operation.
		/// </summary>
		public virtual Task SaveChangesAsync()
		{
			return Task.Run(() => this.SaveChanges());
		}

		/// <summary>
		/// Resets this data context.
		/// </summary>
		public abstract void Reset();

		#endregion

		#region Implementation of IExecuteThreadSafe

		/// <summary>
		/// Executes the specified action thread safe.
		/// </summary>
		public void ExecuteThreadSafe(Action action)
		{
			this.syncRootSemaphore.Wait();
			try
			{
				action();
			}
			finally
			{
				this.syncRootSemaphore.Release();
			}
		}

		/// <summary>
		/// Executes the specified function thread safe.
		/// </summary>
		public TResult ExecuteThreadSafe<TResult>(Func<TResult> func)
		{
			this.syncRootSemaphore.Wait();
			try
			{
				return func();
			}
			finally
			{
				this.syncRootSemaphore.Release();
			}
		}

		/// <summary>
		/// Executes the specified action thread safe.
		/// </summary>
		public async Task ExecuteThreadSafeAsync(Func<Task> asyncAction)
		{
			await this.syncRootSemaphore.WaitAsync().ConfigureAwait(true);
			try
			{
				await asyncAction().ConfigureAwait(false);
			}
			finally
			{
				this.syncRootSemaphore.Release();
			}
		}

		/// <summary>
		/// Executes the specified function thread safe.
		/// </summary>
		public async Task<TResult> ExecuteThreadSafeAsync<TResult>(Func<Task<TResult>> asyncFunc)
		{
			await this.syncRootSemaphore.WaitAsync().ConfigureAwait(true);
			try
			{
				return await asyncFunc().ConfigureAwait(false);
			}
			finally
			{
				this.syncRootSemaphore.Release();
			}
		}

		#endregion

		#region Implementation of IDataSetExtensions

		/// <summary>
		/// Applys the specified <see cref="QueryOptions"/> to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> ApplyOptions<TEntity>(IQueryable<TEntity> source, QueryOptions options)
			where TEntity : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> source, string path)
			where TEntity : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr)
			where TEntity : class
			where TProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, TProperty>> expr)
			where TEntity : class
			where TProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>,
				IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>, IThenIncludeQueryable<TEntity, TThenProperty>
			>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>
			>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IThenIncludeQueryable<TEntity, TThenProperty> ThenInclude<TEntity, TPreviousProperty, TThenProperty>(
			IThenIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
			Expression<Func<TPreviousProperty, TThenProperty>> expr)
			where TEntity : class
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IThenIncludeQueryable<TEntity, TThenProperty> ThenInclude<TEntity, TPreviousProperty, TThenProperty>(
			IThenIncludeQueryable<TEntity, TPreviousProperty> source,
			Expression<Func<TPreviousProperty, TThenProperty>> expr)
			where TEntity : class
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Creates a <see cref="T:System.Collections.Generic.List`1" /> from an <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<List<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.ToList());
		}

		/// <summary>
		/// Creates a array from an <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<TEntity[]> ToArrayAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.ToArray());
		}

		/// <summary>
		/// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<TEntity> FirstOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.FirstOrDefault());
		}

		/// <summary>
		/// Asynchronously returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<TEntity> LastOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.LastOrDefault());
		}

		/// <summary>
		/// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<TEntity> SingleOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.SingleOrDefault());
		}

		/// <summary>
		/// Asynchronously determines whether a sequence contains any elements.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.Any());
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IFutureQuery<TEntity> Future<TEntity>(IQueryable<TEntity> source)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IFutureValue<TEntity> FutureValue<TEntity>(IQueryable<TEntity> source)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" />.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual List<TEntity> ToDetachedList<TEntity>(IQueryable<TEntity> source)
		{
			return source.ToList();
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual Task<List<TEntity>> ToDetachedListAsync<TEntity>(IQueryable<TEntity> source)
		{
			return Task.Run(() => source.ToList());
		}

		#endregion

		#region Explicit Implementation of IDataContextOwner

		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContext IDataContextOwner.DataContext
		{
			[DebuggerStepThrough]
			get { return this; }
		}

		/// <summary>
		/// Gets the <see cref="IDataContextExtensions" />.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContextExtensions IDataContextOwner.Extensions
		{
			[DebuggerStepThrough]
			get { return this; }
		}

		#endregion

		#region Overrides of Object

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return $"{this.name}[#{this.uniqueID:X5}]";
		}

		#endregion
	}
}