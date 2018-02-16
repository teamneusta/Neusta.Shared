namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.Validation;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode;
	using Neusta.Shared.DataAccess.Changes;
	using Neusta.Shared.DataAccess.DataContext;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;
	using Z.BulkOperations;
	using Z.EntityFramework.Extensions;
	using Z.EntityFramework.Plus;

	[PublicAPI]
	public abstract class EntityFrameworkDataContext<TDataContext, TDbContext> : BaseDataContext<TDataContext>, IEntityFrameworkDataContext<TDataContext>
		where TDataContext : EntityFrameworkDataContext<TDataContext, TDbContext>
		where TDbContext : DbContext
	{
		private readonly IDbContextFactory<TDbContext> dbContextFactory;
		private readonly ConcurrentDictionary<Type, IDynamicProperty[]> keyPropertyCache = new ConcurrentDictionary<Type, IDynamicProperty[]>();
		private TDbContext dbContext;
		private MergeOption mergeOption;
		private bool useBulkOperations;

		/// <summary>
		/// Initializes static members of the <see cref="EntityFrameworkDataContext{TDataContext, TDbContext}"/> class.
		/// </summary>
		static EntityFrameworkDataContext()
		{
			ZExtensionsLicenseManager.Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkDataContext{TDataContext,TDbContext}"/> class.
		/// </summary>
		protected EntityFrameworkDataContext(IDbContextFactory<TDbContext> dbContextFactory)
		{
			this.dbContextFactory = dbContextFactory;
			this.dbContext = this.dbContextFactory.RentDbContext();
			this.mergeOption = MergeOption.PreserveChanges;
			this.useBulkOperations = ZExtensionsLicenseManager.IsLicenseValid;
		}

		/// <summary>
		/// Gets the database context.
		/// </summary>
		protected TDbContext DbContext
		{
			[DebuggerStepThrough]
			get { return this.dbContext; }
		}

		/// <summary>
		/// Gets or sets the merge option.
		/// </summary>
		[PublicAPI]
		[DefaultValue(MergeOption.PreserveChanges)]
		public MergeOption MergeOption
		{
			[DebuggerStepThrough]
			get { return this.mergeOption; }
			[DebuggerStepThrough]
			set { this.mergeOption = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to use bulk operations.
		/// </summary>
		[PublicAPI]
		[DefaultValue(true)]
		public bool UseBulkOperations
		{
			[DebuggerStepThrough]
			get { return this.useBulkOperations; }
			[DebuggerStepThrough]
			set { this.useBulkOperations = value; }
		}

		/// <summary>
		/// Gets the DbEntityEntry for the specified entity.
		/// </summary>
		[PublicAPI]
		public virtual DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
			where TEntity : class
		{
			return this.dbContext.Entry(entity);
		}

		/// <summary>
		/// Gets a DbSet for the specified entity type.
		/// </summary>
		[PublicAPI]
		public virtual DbSet<TEntity> Set<TEntity>()
			where TEntity : class, IEntity
		{
			return this.dbContext.Set<TEntity>();
		}

		/// <summary>
		/// Returns an <see cref="T:System.Linq.IQueryable`1" /> that allows to query the database.
		/// </summary>
		[PublicAPI]
		public virtual IQueryable<TEntity> Query<TEntity>(MergeOption mergeOption)
			where TEntity : class, IEntity
		{
			ObjectContext objectContext = ((IObjectContextAdapter)this.dbContext).ObjectContext;
			ObjectSet<TEntity> query = objectContext.CreateObjectSet<TEntity>();
			query.MergeOption = mergeOption;
			return query;
		}

		#region Overrides of DataContext<TDataContext>

		/// <summary>
		/// Gets a value indicating whether this data context is dirty.
		/// </summary>
		public override bool IsDirty => this.dbContext.ChangeTracker.HasChanges();

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		public override IEntity Load(Type entityType, params object[] keyValues)
		{
			return this.dbContext.Set(entityType).Find(keyValues) as IEntity;
		}

		/// <summary>
		/// Loads the entity using the specified key values.
		/// </summary>
		public override TEntity Load<TEntity>(params object[] keyValues)
		{
			return this.dbContext.Set<TEntity>().Find(keyValues);
		}

		/// <summary>
		/// Loads the entity with the specified identifier.
		/// </summary>
		public override TEntity Load<TEntity, TID>(TID id)
		{
			return this.dbContext.Set<TEntity>().Find(id);
		}

		/// <summary>
		/// Returns an <see cref="T:System.Linq.IQueryable`1" /> that allows to query the database.
		/// </summary>
		public override IQueryable<TEntity> Query<TEntity>()
		{
			ObjectContext objectContext = ((IObjectContextAdapter)this.dbContext).ObjectContext;
			ObjectSet<TEntity> query = objectContext.CreateObjectSet<TEntity>();
			query.MergeOption = this.mergeOption;
			return query;
		}

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		public override void Add<TEntity>(TEntity entity)
		{
			this.dbContext.Set(entity.GetType()).Add(entity);
		}

		/// <summary>
		/// Adds the given list of entities of the same type.
		/// </summary>
		public override void AddRange<TEntity>(IEnumerable<TEntity> entities)
		{
			this.dbContext.Set<TEntity>().AddRange(entities);
		}

		/// <summary>
		/// Adds the given list of entities.
		/// </summary>
		public override void AddRange(IEnumerable<IEntity> entities)
		{
			IEnumerable<IGrouping<Type, IEntity>> map = entities.GroupBy(x => x.GetType());
			foreach (IGrouping<Type, IEntity> group in map)
			{
				this.dbContext.Set(group.Key).AddRange(group);
			}
		}


		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		public override void Update<TEntity>(TEntity entity)
		{
			DbEntityEntry entry = this.dbContext.Entry((object)entity);
			if (entry.State == EntityState.Unchanged)
			{
				entry.State = EntityState.Modified;
			}
		}

		/// <summary>
		/// Updates the given list of entities of the same type.
		/// </summary>
		public override void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
		{
			foreach (TEntity entity in entities)
			{
				DbEntityEntry<TEntity> entry = this.Entry(entity);
				if (entry.State == EntityState.Unchanged)
				{
					entry.State = EntityState.Modified;
				}
			}
		}

		/// <summary>
		/// Updates the given list of entities of the same type.
		/// </summary>
		public override void UpdateRange(IEnumerable<IEntity> entities)
		{
			foreach (IEntity entity in entities)
			{
				DbEntityEntry entry = this.Entry((object)entity);
				if (entry.State == EntityState.Unchanged)
				{
					entry.State = EntityState.Modified;
				}
			}
		}

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		public override void Delete<TEntity>(TEntity entity)
		{
			DbEntityEntry entry = this.dbContext.Entry((object)entity);
			switch (entry.State)
			{
				case EntityState.Added:
					entry.State = EntityState.Detached;
					break;
				case EntityState.Unchanged:
				case EntityState.Modified:
					entry.State = EntityState.Deleted;
					break;
			}
		}

		/// <summary>
		/// Deletes the given list of entities of the same type.
		/// </summary>
		public override void DeleteRange<TEntity>(IEnumerable<TEntity> entities)
		{
			foreach (TEntity entity in entities)
			{
				DbEntityEntry<TEntity> entry = this.dbContext.Entry(entity);
				switch (entry.State)
				{
					case EntityState.Added:
						entry.State = EntityState.Detached;
						break;
					case EntityState.Unchanged:
					case EntityState.Modified:
						entry.State = EntityState.Deleted;
						break;
				}
			}
		}

		/// <summary>
		/// Deletes the given list of entities.
		/// </summary>
		public override void DeleteRange(IEnumerable<IEntity> entities)
		{
			foreach (IEntity entity in entities)
			{
				DbEntityEntry entry = this.dbContext.Entry((object)entity);
				switch (entry.State)
				{
					case EntityState.Added:
						entry.State = EntityState.Detached;
						break;
					case EntityState.Unchanged:
					case EntityState.Modified:
						entry.State = EntityState.Deleted;
						break;
				}
			}
		}

		/// <summary>
		/// Attaches the specified entity.
		/// </summary>
		public override void Attach<TEntity>(TEntity entity)
		{
			this.dbContext.Entry((object)entity).State = EntityState.Unchanged;
		}

		/// <summary>
		/// Detaches the specified entity.
		/// </summary>
		public override void Detach<TEntity>(TEntity entity)
		{
			this.dbContext.Entry((object)entity).State = EntityState.Detached;
		}

		/// <summary>
		/// Refreshes the specified entity.
		/// </summary>
		public override void Refresh<TEntity>(TEntity entity)
		{
			ObjectContext objectContext = ((IObjectContextAdapter)this.dbContext).ObjectContext;
			objectContext.Refresh(RefreshMode.StoreWins, entity);
		}

		/// <summary>
		/// Refreshes the specified entities.
		/// </summary>
		public override void RefreshRange<TEntity>(IEnumerable<TEntity> entities)
		{
			ObjectContext objectContext = ((IObjectContextAdapter)this.dbContext).ObjectContext;
			objectContext.Refresh(RefreshMode.StoreWins, entities);
		}

		/// <summary>
		/// Detects changes to all known entities
		/// </summary>
		public override void DetectChanges()
		{
			this.InternalDetectChanges();
		}

		/// <summary>
		/// Gets the current change set.
		/// </summary>
		public override IChangeSet GetChangeSet()
		{
			return this.InternalGetChangeSet();
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public override void SaveChanges()
		{
			if (this.IsDirty)
			{
				this.InternalSaveChanges();
			}
		}

		/// <summary>
		/// Saves the changes as an asynchronous operation.
		/// </summary>
		public override async Task SaveChangesAsync()
		{
			if (this.IsDirty)
			{
				await this.InternalSaveChangesAsync().ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Resets this data context.
		/// </summary>
		public override void Reset()
		{
			// Release context
			var context = Interlocked.Exchange(ref this.dbContext, null);
			if (context != null)
			{
				this.dbContextFactory.ReturnDbContext(context);
			}

			// Retrieve new DbContext
			this.dbContext = this.dbContextFactory.RentDbContext();
		}

		/// <summary>
		/// Applys the specified <see cref="T:System.Linq.QueryOptions" /> to the query.
		/// </summary>
		public override IQueryable<TEntity> ApplyOptions<TEntity>(IQueryable<TEntity> source, QueryOptions options)
		{
			if (options.HasFlag(QueryOptions.NoTracking))
			{
				source = source.AsNoTracking();
			}
			if (options.HasFlag(QueryOptions.AsStreaming))
			{
#pragma warning disable 618
				// Use this for compatibility reasons
				source = source.AsStreaming();
#pragma warning restore 618
			}
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> source, string path)
		{
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				source = source.IncludeOptimizedByPath(path);
			}
			else
			{
				source = source.Include(path);
			}
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> expr)
		{
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				source = source.IncludeOptimized(expr);
			}
			else
			{
				source = source.Include(expr);
			}
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, IEnumerable<TProperty>>> expr)
		{
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				source = source.IncludeOptimized(expr);
			}
			else
			{
				source = source.Include(expr);
			}
			return source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, IEnumerable<TProperty>>> expr, Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>, IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
		{
			IQueryable<TEntity> result;
			if (then != null)
			{
				throw new NotSupportedException();
			}
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				result = source.IncludeOptimized(expr);
			}
			else
			{
				result = source.Include(expr);
			}
			return result;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, IEnumerable<TProperty>>> expr, Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
		{
			IQueryable<TEntity> result;
			if (then != null)
			{
				throw new NotSupportedException();
			}
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				result = source.IncludeOptimized(expr);
			}
			else
			{
				result = source.Include(expr);
			}
			return result;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> expr, Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
		{
			IQueryable<TEntity> result;
			if (then != null)
			{
				throw new NotSupportedException();
			}
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				result = source.IncludeOptimized(expr);
			}
			else
			{
				result = source.Include(expr);
			}
			return result;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		public override IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> expr, Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
		{
			IQueryable<TEntity> result;
			if (then != null)
			{
				throw new NotSupportedException();
			}
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				result = source.IncludeOptimized(expr);
			}
			else
			{
				result = source.Include(expr);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="T:System.Collections.Generic.List`1" /> from an <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		public override async Task<List<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> source)
		{
			return await source.ToListAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Creates a array from an <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		public override Task<TEntity[]> ToArrayAsync<TEntity>(IQueryable<TEntity> source)
		{
			return source.ToArrayAsync();
		}

		/// <summary>
		/// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		public override Task<TEntity> FirstOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return source.FirstOrDefaultAsync();
		}

		/// <summary>
		/// Asynchronously returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		public override Task<TEntity> LastOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return source.Reverse().FirstOrDefaultAsync();
		}

		/// <summary>
		/// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		public override Task<TEntity> SingleOrDefaultAsync<TEntity>(IQueryable<TEntity> source)
		{
			return source.SingleOrDefaultAsync();
		}

		/// <summary>
		/// Asynchronously determines whether a sequence contains any elements.
		/// </summary>
		public override Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> source)
		{
			return source.AnyAsync();
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		public override IFutureQuery<TEntity> Future<TEntity>(IQueryable<TEntity> source)
		{
			IFutureQuery<TEntity> wrapper;
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				QueryFutureEnumerable<TEntity> future = source.Future();
				wrapper = new FutureQueryWrapper<TEntity>(this, future);
			}
			else
			{
				wrapper = new StandardQueryWrapper<TEntity>(this, source);
			}
			return wrapper;
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		public override IFutureValue<TEntity> FutureValue<TEntity>(IQueryable<TEntity> source)
		{
			IFutureValue<TEntity> wrapper;
			if (ZExtensionsLicenseManager.IsLicenseValid)
			{
				QueryFutureValue<TEntity> future = source.FutureValue();
				wrapper = new FutureValueWrapper<TEntity>(this, future);
			}
			else
			{
				wrapper = new StandardValueWrapper<TEntity>(this, source);
			}
			return wrapper;
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" />.
		/// </summary>
		public override List<TEntity> ToDetachedList<TEntity>(IQueryable<TEntity> source)
		{
			bool autoDetectChangesEnabled = this.dbContext.Configuration.AutoDetectChangesEnabled;
			bool proxyCreationEnabled = this.dbContext.Configuration.ProxyCreationEnabled;
			try
			{
				this.dbContext.Configuration.AutoDetectChangesEnabled = false;
				this.dbContext.Configuration.ProxyCreationEnabled = false;
				return source.ToList();
			}
			finally
			{
				this.dbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
				this.dbContext.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
			}
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		public override async Task<List<TEntity>> ToDetachedListAsync<TEntity>(IQueryable<TEntity> source)
		{
			bool autoDetectChangesEnabled = this.dbContext.Configuration.AutoDetectChangesEnabled;
			bool proxyCreationEnabled = this.dbContext.Configuration.ProxyCreationEnabled;
			try
			{
				this.dbContext.Configuration.AutoDetectChangesEnabled = false;
				this.dbContext.Configuration.ProxyCreationEnabled = false;
				return await source.ToListAsync().ConfigureAwait(false);
			}
			finally
			{
				this.dbContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
				this.dbContext.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
			}
		}

		#endregion

		#region Implementation of IEntityFrameworkDataContext<TDataContext, TDbContext>

		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		public virtual void AddBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				this.dbContext.BulkInsert(entities, this.ConfigureBulkOperation);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				this.AddRange(entities);
			}
		}

		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		public virtual async Task AddBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				await this.dbContext.BulkInsertAsync(entities, this.ConfigureBulkOperation).ConfigureAwait(false);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				this.AddRange(entities);
			}
		}

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		public virtual void UpdateBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				this.dbContext.BulkUpdate(entities, this.ConfigureBulkOperation);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				this.UpdateRange(entities);
			}
		}

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		public virtual async Task UpdateBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				await this.dbContext.BulkUpdateAsync(entities, this.ConfigureBulkOperation).ConfigureAwait(false);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				this.UpdateRange(entities);
			}
		}

		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		public virtual void DeleteBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				this.dbContext.BulkDelete(entities, this.ConfigureBulkOperation);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Detached);
			}
			else
			{
				this.DeleteRange(entities);
			}
		}
		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		public virtual async Task DeleteBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				await this.dbContext.BulkDeleteAsync(entities, this.ConfigureBulkOperation).ConfigureAwait(false);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Detached);
			}
			else
			{
				this.DeleteRange(entities);
			}
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public virtual void MergeBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				this.dbContext.BulkMerge(entities, this.ConfigureBulkOperation);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public virtual async Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				await this.dbContext.BulkMergeAsync(entities, this.ConfigureBulkOperation).ConfigureAwait(false);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public virtual void MergeBulk<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				this.dbContext.BulkMerge(entities, delegate (EntityBulkOperation<TEntity> operation)
				{
					this.ConfigureBulkOperation(operation);
					operation.ColumnPrimaryKeyExpression = primaryKeyExpression;
				});
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public virtual async Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
			where TEntity : class, IEntity
		{
			if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
			{
				entities = this.EnforceMultipleEnumerableCollection(entities);
				await this.dbContext.BulkMergeAsync(entities, delegate(EntityBulkOperation<TEntity> operation)
				{
					this.ConfigureBulkOperation(operation);
					operation.ColumnPrimaryKeyExpression = primaryKeyExpression;
				}).ConfigureAwait(false);
				entities.ForEach(entity => this.dbContext.Entry(entity).State = EntityState.Unchanged);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#endregion

		#region Overrides of DisposableObject

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// Release context
			var context = Interlocked.Exchange(ref this.dbContext, null);
			if (context != null)
			{
				this.dbContextFactory.ReturnDbContext(context);
			}
		}

		#endregion

		#region Protected Methods

		protected IChangeSet InternalGetChangeSet()
		{
			this.InternalDetectChanges();

			IEnumerable<DbEntityEntry> changeTrackerEntries = this.dbContext.ChangeTracker.Entries();
			IEnumerable<DbEntityEntry> updatedEntries = changeTrackerEntries.Where(e => e.State == EntityState.Modified && e.Entity != null);

			IList<IEntityChange> updateChanges = new List<IEntityChange>();
			foreach (DbEntityEntry dbEntityEntry in updatedEntries)
			{
				IList<PropertyChange> changes = new List<PropertyChange>();
				foreach (string propertyName in dbEntityEntry.CurrentValues.PropertyNames)
				{
					DbPropertyEntry propertyEntry = dbEntityEntry.Property(propertyName);
					if (propertyEntry.IsModified && !Equals(propertyEntry.CurrentValue, propertyEntry.OriginalValue))
					{
						changes.Add(new PropertyChange(propertyName, propertyEntry.OriginalValue, propertyEntry.CurrentValue));
					}
				}
				updateChanges.Add(EntityChange.CreateUpdateChange(dbEntityEntry.Entity, changes));
			}

			IEnumerable<IEntityChange> addChanges = changeTrackerEntries.Where(e => e.State == EntityState.Added && e.Entity != null)
				.Select(e => EntityChange.CreateAddedChange(e.Entity));

			IEnumerable<IEntityChange> deleteChanges = changeTrackerEntries.Where(e => e.State == EntityState.Deleted && e.Entity != null)
				.Select(n => EntityChange.CreateDeleteChange(n.Entity));

			var result = new List<IEntityChange>(addChanges);
			result.AddRange(deleteChanges);
			result.AddRange(updateChanges);

			return new ChangeSet(this.dbContext.GetType(), result);
		}

		protected virtual void InternalDetectChanges()
		{
			DbChangeTracker changeTracker = this.dbContext.ChangeTracker;
			changeTracker.DetectChanges();

			if (Logger.IsDebugEnabled)
			{
				DbEntityEntry[] entries = changeTracker.Entries().ToArray();
				int count = entries.Length;
				if (count > 0)
				{
					int added = entries.Count(x => x.State == EntityState.Added);
					int deleted = entries.Count(x => x.State == EntityState.Deleted);
					int modified = entries.Count(x => x.State == EntityState.Modified);
					int changes = added + deleted + modified;
					if (changes > 0)
					{
						Logger.Debug(() => $"Changes detected: Change tracker has {count} entries and {changes} changes (ADD:{added}|CHG:{modified}|DEL:{deleted}),");
					}
				}
			}
		}

		protected virtual void InternalSaveChanges()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
				{
					if (!this.dbContext.Configuration.AutoDetectChangesEnabled)
					{
						this.InternalDetectChanges();
					}
					this.dbContext.BulkSaveChanges(false, this.ConfigureBulkOperation);
				}
				else
				{
					this.dbContext.SaveChanges();
				}
				Logger.Debug(() => string.Format("SaveChanges successful, {0}ms", stopwatch.ElapsedMilliseconds));
			}
			catch (DbEntityValidationException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (DbUpdateException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (InvalidOperationException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					ex = ex.InnerException;
				}
				Logger.Error(ex.Message, ex);
				throw;
			}
		}

		protected virtual async Task InternalSaveChangesAsync()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				if (this.useBulkOperations && ZExtensionsLicenseManager.IsLicenseValid)
				{
					if (!this.dbContext.Configuration.AutoDetectChangesEnabled)
					{
						this.InternalDetectChanges();
					}
					await this.dbContext.BulkSaveChangesAsync(false, this.ConfigureBulkOperation).ConfigureAwait(false);
				}
				else
				{
					await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
				}
				Logger.Debug(() => string.Format("SaveChanges successful, {0}ms", stopwatch.ElapsedMilliseconds));
			}
			catch (DbEntityValidationException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (DbUpdateException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (InvalidOperationException ex)
			{
				Logger.Error(ex.Message, ex);
				throw;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					ex = ex.InnerException;
				}
				Logger.Error(ex.Message, ex);
				throw;
			}
		}

		protected ICollection<T> EnforceMultipleEnumerableCollection<T>(IEnumerable<T> entities)
		{
			var list = entities as ICollection<T>;
			if (list == null)
			{
				list = entities.ToList();
			}
			return list;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Configures the bulk operation.
		/// </summary>
		private void ConfigureBulkOperation(BulkOperation operation)
		{
			if (ContextLogger.IsLoggingEnabled)
			{
				operation.Log = ContextLogger.LogMessage;
			}
		}

		#endregion
	}
}