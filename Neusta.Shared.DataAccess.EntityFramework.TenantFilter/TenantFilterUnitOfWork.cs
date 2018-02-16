namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter
{
	using System;
	using System.Diagnostics;
	using Neusta.Shared.DataAccess.UnitOfWork;

	public class TenantFilterUnitOfWork : BaseUnitOfWork, ITenantFilterUnitOfWork
	{
		private bool tenantFilterEnabled;
		private long? tenantID;

		/// <summary>
		/// Initializes a new instance of the <see cref="TenantFilterUnitOfWork"/> class.
		/// </summary>
		public TenantFilterUnitOfWork()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TenantFilterUnitOfWork"/> class.
		/// </summary>
		public TenantFilterUnitOfWork(IServiceProvider serviceProvider, IUnitOfWork parentUnitOfWork = null)
			: base(serviceProvider, parentUnitOfWork)
		{
			if (parentUnitOfWork != null)
			{
				if (parentUnitOfWork is TenantFilterUnitOfWork tenantFilterUnitOfWork)
				{
					this.tenantFilterEnabled = tenantFilterUnitOfWork.tenantFilterEnabled;
					this.tenantID = tenantFilterUnitOfWork.tenantID;
				}
				else
				{
					this.tenantFilterEnabled = parentUnitOfWork.GetProperty<bool>(TenantFilterUnitOfWorkPropertyKeys.TenantFilterEnabledKey, false);
					if (this.tenantFilterEnabled)
					{
						this.tenantID = parentUnitOfWork.GetProperty<long>(TenantFilterUnitOfWorkPropertyKeys.TenantIDKey);
					}
				}
			}
		}

		#region Implementation of ITenantIDProvider

		public bool TenantFilterEnabled
		{
			[DebuggerStepThrough]
			get { return this.tenantFilterEnabled; }
		}

		/// <summary>
		/// Gets the tenant identifier.
		/// </summary>
		public long? TenantID
		{
			[DebuggerStepThrough]
			get { return this.tenantID; }
		}

		#endregion

		#region Implementation of IInitializeTenantFilter

		/// <summary>
		/// Enables the tenant filter.
		/// </summary>
		public void EnableTenantFilter()
		{
			this.tenantFilterEnabled = true;
			this.SetProperty(TenantFilterUnitOfWorkPropertyKeys.TenantFilterEnabledKey, true);
		}

		/// <summary>
		/// Disables the tenant filter.
		/// </summary>
		public void DisableTenantFilter()
		{
			this.tenantFilterEnabled = false;
			this.SetProperty(TenantFilterUnitOfWorkPropertyKeys.TenantFilterEnabledKey, false);
		}

		/// <summary>
		/// Initializes the tenant filter.
		/// </summary>
		public void InitializeTenantFilter(long tenantID)
		{
			this.tenantFilterEnabled = true;
			this.SetProperty(TenantFilterUnitOfWorkPropertyKeys.TenantFilterEnabledKey, true);

			if (this.tenantID.HasValue)
			{
				if (this.tenantID == tenantID)
				{
					return;
				}
				this.tenantID = tenantID;
				this.SetProperty(TenantFilterUnitOfWorkPropertyKeys.TenantIDKey, tenantID);
				this.FlushRepositories();
				this.FlushDataContexts();
			}
			else
			{
				this.tenantID = tenantID;
				this.SetProperty(TenantFilterUnitOfWorkPropertyKeys.TenantIDKey, tenantID);
				foreach (IDataContext dataContext in this.DataContexts)
				{
					if (dataContext is IInitializeTenantFilter intf)
					{
						intf.InitializeTenantFilter(tenantID);
					}
				}
			}
		}

		#endregion

		#region Overrides of UnitOfWork

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public override IUnitOfWork Clone()
		{
			return new TenantFilterUnitOfWork(this.ServiceProvider, this);
		}

		// ReSharper disable SuspiciousTypeConversion.Global

		/// <summary>
		/// Requests the data context from factory.
		/// </summary>
		protected override IDataContext CreateDataContext(Type dataContextType)
		{
			IDataContext dataContext = base.CreateDataContext(dataContextType);
			if (this.tenantFilterEnabled && (dataContext is IInitializeTenantFilter intf))
			{
				intf.InitializeTenantFilter(this.tenantID.GetValueOrDefault());
			}
			return dataContext;
		}

		/// <summary>
		/// Requests the data repository from factory.
		/// </summary>
		protected override IDataRepository CreateDataRepository(Type dataRepositoryType)
		{
			IDataRepository repository = base.CreateDataRepository(dataRepositoryType);
			if (this.tenantFilterEnabled && (repository is IInitializeTenantFilter intf))
			{
				intf.InitializeTenantFilter(this.tenantID.GetValueOrDefault());
			}
			return repository;
		}

		// ReSharper restore SuspiciousTypeConversion.Global

		#endregion

		#region Overrides of Object

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			if (this.tenantFilterEnabled)
			{
				return $"{this.GetType().Name}[#{this.UniqueID},TID:{this.tenantID}]";
			}
			return base.ToString();
		}

		#endregion
	}
}