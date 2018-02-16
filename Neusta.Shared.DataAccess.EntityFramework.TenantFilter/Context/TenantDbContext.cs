namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Context
{
	using System;
	using System.Collections;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;
	using Neusta.Shared.DataAccess.EntityFramework.Context;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Conventions;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Utils;

	public abstract class TenantDbContext : OptimizedDbContext, ITenantIDProvider, IInitializeTenantFilter
	{
		private bool enableTenantFilter;
		private long? tenantID;

		/// <summary>
		/// Initializes a new instance of the <see cref="TenantDbContext"/> class.
		/// </summary>
		protected TenantDbContext()
		{
		}

		/// <summary>
		/// Constructs a new context instance using conventions to create the name of the database to
		/// which a connection will be made, and initializes it from the given model.
		/// The by-convention name is the full name (namespace + class name) of the derived context class.
		/// </summary>
		protected TenantDbContext(DbCompiledModel model)
			: base(model)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made.
		/// </summary>
		protected TenantDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made, and initializes it from the given model.
		/// </summary>
		protected TenantDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected TenantDbContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database,
		/// and initializes it from the given model.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected TenantDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
		{
		}

		/// <summary>
		/// Constructs a new context instance around an existing ObjectContext.
		/// </summary>
		protected TenantDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
			: base(objectContext, dbContextOwnsObjectContext)
		{
		}

		#region Implementation of ITenantIDProvider

		/// <summary>
		/// Gets a value indicating whether the tenant filter is enabled.
		/// </summary>
		public bool TenantFilterEnabled
		{
			[DebuggerStepThrough]
			get { return this.enableTenantFilter; }
		}

		/// <summary>
		/// Gets the current tenant ID.
		/// </summary>
		public long? TenantID
		{
			[DebuggerStepThrough]
			get { return this.tenantID; }
		}

		#endregion

		#region Implementation of IInitializeTenantFilter

		/// <summary>
		/// Initializes the tenant filter.
		/// </summary>
		public void InitializeTenantFilter(long tenantID)
		{
			if (this.tenantID.HasValue)
			{
				if (this.tenantID == tenantID)
				{
					return;
				}
				throw new InvalidOperationException();
			}

			this.enableTenantFilter = true;
			this.tenantID = tenantID;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Enables the tenant filter.
		/// </summary>
		internal void EnableTenantFilter()
		{
			this.enableTenantFilter = true;
		}

		/// <summary>
		/// Disables the tenant filter.
		/// </summary>
		internal void DisableTenantFilter()
		{
			this.enableTenantFilter = false;
		}

		#endregion

		#region Configuration Sets

		/// <summary>
		/// Gets the entity set of <see cref="TenantEntity"/>s.
		/// </summary>
		public IDbSet<TenantEntity> Tenants { get; set; }

		#endregion

		#region Overrides of OptimizedDbContext

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Add(
				new AttributeToTableAnnotationConvention<TenantFilterAttribute, string>(
					TenantFilterConstants.AnnotationName,
					(type, attributes) => attributes.Single().ColumnName));

			modelBuilder.Conventions.Add<NoCascadeDeleteOnTenantForeignKeys>();
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		internal void UpdateTenantID()
		{
			if (this.tenantID.HasValue)
			{
				DbChangeTracker changeTracker = this.ChangeTracker;
				if (!this.Configuration.AutoDetectChangesEnabled)
				{
					changeTracker.DetectChanges();
				}
				this.UpdateTenantID(changeTracker
						.Entries()
						.Where(p => p.State == EntityState.Added | p.State == EntityState.Modified)
						.Select(entry => entry.Entity),
					this.tenantID.Value);
			}
		}

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		internal Task UpdateTenantIDAsync()
		{
			if (this.tenantID.HasValue)
			{
				DbChangeTracker changeTracker = this.ChangeTracker;
				if (!this.Configuration.AutoDetectChangesEnabled)
				{
					changeTracker.DetectChanges();
				}
				return this.UpdateTenantIDAsync(changeTracker
						.Entries()
						.Where(p => p.State == EntityState.Added | p.State == EntityState.Modified)
						.Select(entry => entry.Entity),
					this.tenantID.Value);
			}
			return Task.CompletedTask;
		}

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		internal void UpdateTenantID(IEnumerable entities)
		{
			if (this.tenantID.HasValue)
			{
				this.UpdateTenantID(entities, this.tenantID.Value);
			}
		}

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		internal Task UpdateTenantIDAsync(IEnumerable entities)
		{
			if (this.tenantID.HasValue)
			{
				return this.UpdateTenantIDAsync(entities, this.tenantID.Value);
			}
			else
			{
				return Task.CompletedTask;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		private void UpdateTenantID(IEnumerable entities, long tenantIDValue)
		{
			bool tenantResolved = false;
			TenantEntity tenantEntity = null;
			foreach (object entityCandidate in entities)
			{
				if (entityCandidate is ITenantAwareEntity entity && entity.TenantID != tenantIDValue)
				{
					if (!tenantResolved)
					{
						DbEntityEntry<ITenantAwareEntity> entityEntry = this.Entry(entity);
						if (entityEntry.State != EntityState.Detached)
						{
							DbPropertyEntry<ITenantAwareEntity, long> tenantProperty = entityEntry.Property(x => x.TenantID);
							tenantProperty.CurrentValue = tenantIDValue;
							DbReferenceEntry<ITenantAwareEntity, TenantEntity> tenantReference = entityEntry.Reference(x => x.Tenant);
							tenantReference.Load();
							tenantEntity = tenantReference.CurrentValue;
						}
						else
						{
							entity.TenantID = tenantIDValue;
						}
						tenantResolved = true;
					}
					else
					{
						entity.TenantID = tenantIDValue;
						entity.Tenant = tenantEntity;
					}
				}
			}
		}

		/// <summary>
		/// Updates the tenant identifier.
		/// </summary>
		private async Task UpdateTenantIDAsync(IEnumerable entities, long tenantIDValue)
		{
			bool tenantResolved = false;
			TenantEntity tenantEntity = null;
			foreach (object entityCandidate in entities)
			{
				if (entityCandidate is ITenantAwareEntity entity && entity.TenantID != tenantIDValue)
				{
					if (!tenantResolved)
					{
						DbEntityEntry<ITenantAwareEntity> entityEntry = this.Entry(entity);
						if (entityEntry.State != EntityState.Detached)
						{
							DbPropertyEntry<ITenantAwareEntity, long> tenantProperty = entityEntry.Property(x => x.TenantID);
							tenantProperty.CurrentValue = tenantIDValue;
							DbReferenceEntry<ITenantAwareEntity, TenantEntity> tenantReference = entityEntry.Reference(x => x.Tenant);
							await tenantReference.LoadAsync().ConfigureAwait(false);
							tenantEntity = tenantReference.CurrentValue;
						}
						else
						{
							entity.TenantID = tenantIDValue;
						}
						tenantResolved = true;
					}
					else
					{
						entity.TenantID = tenantIDValue;
						entity.Tenant = tenantEntity;
					}
				}
			}
		}
		#endregion
	}
}