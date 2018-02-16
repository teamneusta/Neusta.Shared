namespace Neusta.Shared.DataAccess.EntityFramework.Factory
{
	using System;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;

	[PublicAPI]
	public abstract class BaseDbContextFactory<TDbContextFactory, TDbContext> : Singleton<TDbContextFactory>, IDbContextFactory<TDbContext>
		where TDbContextFactory : BaseDbContextFactory<TDbContextFactory, TDbContext>
		where TDbContext : DbContext, new()
	{
		private static readonly IDynamicConstructor<TDbContext> dbContextDefaultCtor;
		private static readonly IDynamicConstructor<TDbContext> dbContextNameOrConnectionStringCtor;
		private static readonly IDynamicConstructor<TDbContext> dbContextConnectionCtor;

		private readonly string nameOrConnectionString;
		private IsolationLevel defaultIsolationLevel;

		/// <summary>
		/// Initializes static members of the <see cref="BaseDbContextFactory{TDbContextFactory,TDbContext}" /> class.
		/// </summary>
		static BaseDbContextFactory()
		{
			ZExtensionsLicenseManager.Initialize();

			dbContextDefaultCtor = DynamicConstructor.For<TDbContext>();
			dbContextNameOrConnectionStringCtor = DynamicConstructor.For<TDbContext>(new[] { typeof(string) });
			dbContextConnectionCtor = DynamicConstructor.For<TDbContext>(new[] { typeof(DbConnection), typeof(bool) });
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDbContextFactory{TDbContextFactory,TDbContext}"/> class.
		/// </summary>
		protected BaseDbContextFactory()
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDbContextFactory{TDbContextFactory, TDbContext}"/> class.
		/// </summary>
		protected BaseDbContextFactory(string nameOrConnectionString)
		{
			this.nameOrConnectionString = nameOrConnectionString;

			// ReSharper disable once VirtualMemberCallInConstructor
			this.Initialize();
		}

		/// <summary>
		/// Gets the name or connection string.
		/// </summary>
		public string NameOrConnectionString
		{
			[DebuggerStepThrough]
			get { return this.nameOrConnectionString; }
		}

		/// <summary>
		/// Gets or sets the default isolation level.
		/// </summary>
		[DefaultValue(IsolationLevel.ReadUncommitted)]
		public IsolationLevel DefaultIsolationLevel
		{
			[DebuggerStepThrough]
			get { return this.defaultIsolationLevel; }
			[DebuggerStepThrough]
			set { this.defaultIsolationLevel = value; }
		}

		#region Implementation of IDbContextFactory<TDbContext>

		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		public virtual TDbContext CreateDbContext()
		{
			if (string.IsNullOrEmpty(this.nameOrConnectionString))
			{
				return dbContextDefaultCtor.Invoke();
			}
			return dbContextNameOrConnectionStringCtor.Invoke(this.nameOrConnectionString);
		}

		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		public virtual TDbContext CreateDbContext(DbConnection connection, bool ownsConnection)
		{
			return dbContextConnectionCtor.Invoke(connection, ownsConnection);
		}

		/// <summary>
		/// Rents a <see cref="TDbContext" /> from the factory.
		/// </summary>
		public virtual TDbContext RentDbContext()
		{
			// This allows pooling, but EF6 does not have this feature
			var context = this.CreateDbContext();
			switch (this.defaultIsolationLevel)
			{
				case IsolationLevel.ReadUncommitted:
					context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
					break;
				case IsolationLevel.ReadCommitted:
					context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ COMMITTED;");
					break;
				case IsolationLevel.RepeatableRead:
					context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;");
					break;
				case IsolationLevel.Chaos:
					context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL CHAOS;");
					break;
				case IsolationLevel.Snapshot:
					context.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL SNAPSHOT;");
					break;
			}
			return context;
		}

		/// <summary>
		/// Returns the given a <see cref="TDbContext" /> to the factory.
		/// </summary>
		public void ReturnDbContext(TDbContext dbContext)
		{
			// This allows pooling, but EF6 does not have this feature
			dbContext.Dispose();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected virtual void Initialize()
		{
			this.defaultIsolationLevel = IsolationLevel.ReadUncommitted;
		}

		#endregion
	}
}