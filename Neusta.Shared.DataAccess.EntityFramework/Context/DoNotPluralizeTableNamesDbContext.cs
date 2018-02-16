namespace Neusta.Shared.DataAccess.EntityFramework.Context
{
	using System;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class DoNotPluralizeTableNamesDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FixUtcDateTimeKindDbContext"/> class.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext()
		{
		}

		/// <summary>
		/// Constructs a new context instance using conventions to create the name of the database to
		/// which a connection will be made, and initializes it from the given model.
		/// The by-convention name is the full name (namespace + class name) of the derived context class.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(DbCompiledModel model)
			: base(model)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made, and initializes it from the given model.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database,
		/// and initializes it from the given model.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
		{
		}

		/// <summary>
		/// Constructs a new context instance around an existing ObjectContext.
		/// </summary>
		protected DoNotPluralizeTableNamesDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
			: base(objectContext, dbContextOwnsObjectContext)
		{
		}

		#region Overrides of DbContext

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		/// before the model has been locked down and used to initialize the context.
		/// </summary>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		#endregion
	}
}