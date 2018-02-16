namespace Neusta.Shared.DataAccess.EntityFramework.Context
{
	using System;
	using System.ComponentModel;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Conventions;
	using Neusta.Shared.DataAccess.EntityFramework.Utils.Internal;
	using Neusta.Shared.Logging;

	[PublicAPI]
	public abstract class OptimizedDbContext : DoNotPluralizeTableNamesDbContext
	{
		private static readonly ILogger logger = LogManager.GetLogger(typeof(OptimizedDbContext));

		private bool useExtendedDateTimeColumnType = true;

		/// <summary>
		/// Initializes static members of the <see cref="OptimizedDbContext"/> class.
		/// </summary>
		static OptimizedDbContext()
		{
			ZExtensionsLicenseManager.Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OptimizedDbContext"/> class.
		/// </summary>
		protected OptimizedDbContext()
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance using conventions to create the name of the database to
		/// which a connection will be made, and initializes it from the given model.
		/// The by-convention name is the full name (namespace + class name) of the derived context class.
		/// </summary>
		protected OptimizedDbContext(DbCompiledModel model)
			: base(model)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made.
		/// </summary>
		protected OptimizedDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made, and initializes it from the given model.
		/// </summary>
		protected OptimizedDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected OptimizedDbContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database,
		/// and initializes it from the given model.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected OptimizedDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Constructs a new context instance around an existing ObjectContext.
		/// </summary>
		protected OptimizedDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
			: base(objectContext, dbContextOwnsObjectContext)
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateConfiguration(this.Configuration);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to use "datetime2" as date time column type.
		/// </summary>
		[PublicAPI]
		[DefaultValue(true)]
		public bool UseExtendedDateTimeColumnType
		{
			[DebuggerStepThrough]
			get { return this.useExtendedDateTimeColumnType; }
			[DebuggerStepThrough]
			set { this.useExtendedDateTimeColumnType = value; }
		}

		#region Protected Methods

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		protected virtual void UpdateConfiguration(DbContextConfiguration configuration)
		{
			configuration.AutoDetectChangesEnabled = true;
			configuration.ProxyCreationEnabled = true;
			configuration.LazyLoadingEnabled = true;
			configuration.UseDatabaseNullSemantics = true;

			// Logging of SQL statements
			if (ContextLogger.IsLoggingEnabled)
			{
				this.Database.Log = ContextLogger.LogMessage;
			}
		}

		#endregion

		#region Overrides of DbContext

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		/// before the model has been locked down and used to initialize the context.  The default
		/// implementation of this method does nothing, but it can be overridden in a derived class
		/// such that the model can be further configured before it is locked down.
		/// </summary>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			if (this.useExtendedDateTimeColumnType)
			{
				modelBuilder.Conventions.Add<ExtendedDateTimeColumnTypeConvention>();
			}

			modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
			modelBuilder.Conventions.Add(new DecimalPropertyConvention(36, 12));
			modelBuilder.Conventions.Add<DecimalPrecisionAttributeConvention>();

			modelBuilder.Conventions.Add<DateAttributeConvention>();

			modelBuilder.Conventions.Add<ForeignKeyNamingConvention>();
			modelBuilder.Conventions.Add<PrimaryKeyNamingConvention>();
			modelBuilder.Conventions.Add<DatabaseGeneratedIdentityConvention>();

			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}

		#endregion
	}
}