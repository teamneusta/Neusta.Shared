namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Context;

	[PublicAPI]
	public abstract class TenantDataContext<TDataContext, TDbContext> : EntityFrameworkDataContext<TDataContext, TDbContext>, ITenantDataContext
		where TDataContext : TenantDataContext<TDataContext, TDbContext>
		where TDbContext : TenantDbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TenantDataContext{TDataContext, TDbContext}"/> class.
		/// </summary>
		protected TenantDataContext(ITenantDbContextFactory<TDbContext> dbContextFactory)
			: base(dbContextFactory)
		{
			ITenantDbContextFactory<TDbContext> intf = dbContextFactory;
			if (intf == null || intf.EnableTenantFilter)
			{
				this.DbContext.EnableTenantFilter();
			}
		}

		#region Implementation of ITenantIDProvider

		/// <summary>
		/// Gets a value indicating whether the tenant filter is enabled.
		/// </summary>
		public bool TenantFilterEnabled
		{
			[DebuggerStepThrough]
			get { return this.DbContext.TenantFilterEnabled; }
		}

		/// <summary>
		/// Gets the current tenant ID.
		/// </summary>
		public long? TenantID
		{
			[DebuggerStepThrough]
			get { return this.DbContext.TenantID; }
		}

		#endregion

		#region Implementation of ITenantDataContext

		/// <summary>
		/// Enables the tenant filter.
		/// </summary>
		public void EnableTenantFilter()
		{
			this.DbContext.EnableTenantFilter();
		}

		/// <summary>
		/// Disables the tenant filter.
		/// </summary>
		public void DisableTenantFilter()
		{
			this.DbContext.DisableTenantFilter();
		}

		#endregion

		#region Implementation of IInitializeTenantFilter

		/// <summary>
		/// Initializes the tenant filter.
		/// </summary>
		public void InitializeTenantFilter(long tenantID)
		{
			this.DbContext.InitializeTenantFilter(tenantID);
		}

		#endregion

		#region Overrides of EntityFrameworkDataContext<TDataContext,TDbContext>

		/// <summary>
		/// Creates this instance.
		/// </summary>
		public override TEntity Create<TEntity>()
		{
			var entity = base.Create<TEntity>();
			if (this.TenantFilterEnabled)
			{
				if (entity is ITenantAwareEntity intf)
				{
					intf.TenantID = this.DbContext.TenantID.GetValueOrDefault(-1);
				}
			}
			return entity;
		}

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		public override void Add<TEntity>(TEntity entity)
		{
			if (this.TenantFilterEnabled)
			{
				if (entity is ITenantAwareEntity intf && intf.TenantID <= 0)
				{
					intf.TenantID = this.DbContext.TenantID.GetValueOrDefault(-1);
				}
			}
			base.Add(entity);
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		public override void Update<TEntity>(TEntity entity)
		{
			if (this.TenantFilterEnabled)
			{
				if (entity is ITenantAwareEntity intf && intf.TenantID <= 0)
				{
					intf.TenantID = this.DbContext.TenantID.GetValueOrDefault(-1);
				}
			}
			base.Update(entity);
		}

		#endregion

		#region Overrides of EntityFrameworkDataContext<TDataContext,TDbContext>

		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		public override void AddBulk<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			this.DbContext.UpdateTenantID(entities);
			base.AddBulk(entities);
		}

		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		public override async Task AddBulkAsync<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			await this.DbContext.UpdateTenantIDAsync(entities).ConfigureAwait(false);
			await base.AddBulkAsync(entities).ConfigureAwait(false);
		}

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		public override void UpdateBulk<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			this.DbContext.UpdateTenantID(entities);
			base.UpdateBulk(entities);
		}

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		public override async Task UpdateBulkAsync<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			await this.DbContext.UpdateTenantIDAsync(entities).ConfigureAwait(false); ;
			await base.UpdateBulkAsync(entities).ConfigureAwait(false);
		}

		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		public override void DeleteBulk<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			this.DbContext.UpdateTenantID(entities);
			base.DeleteBulk(entities);
		}

		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		public override async Task DeleteBulkAsync<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			await this.DbContext.UpdateTenantIDAsync(entities).ConfigureAwait(false);
			await base.DeleteBulkAsync(entities).ConfigureAwait(false);
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public override void MergeBulk<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			this.DbContext.UpdateTenantID(entities);
			base.MergeBulk(entities);
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public override async Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			await this.DbContext.UpdateTenantIDAsync(entities).ConfigureAwait(false);
			await base.MergeBulkAsync(entities).ConfigureAwait(false);
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public override void MergeBulk<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			this.DbContext.UpdateTenantID(entities);
			base.MergeBulk(entities, primaryKeyExpression);
		}

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		public override async Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
		{
			entities = this.EnforceMultipleEnumerableCollection(entities);
			await this.DbContext.UpdateTenantIDAsync(entities).ConfigureAwait(false);
			await base.MergeBulkAsync(entities, primaryKeyExpression).ConfigureAwait(false);
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		protected override void InternalSaveChanges()
		{
			this.DbContext.UpdateTenantID();
			base.InternalSaveChanges();
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		protected override async Task InternalSaveChangesAsync()
		{
			await this.DbContext.UpdateTenantIDAsync().ConfigureAwait(false);
			await base.InternalSaveChangesAsync().ConfigureAwait(false);
		}

		#endregion
	}
}