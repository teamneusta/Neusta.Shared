namespace Neusta.Shared.DataAccess.EntityFramework.Context
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.Core.Objects;
	using System.Data.Entity.Infrastructure;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.EntityFramework.Conventions;

	[PublicAPI]
	public abstract class FixUtcDateTimeKindDbContext : DoNotPluralizeTableNamesDbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FixUtcDateTimeKindDbContext"/> class.
		/// </summary>
		protected FixUtcDateTimeKindDbContext()
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance using conventions to create the name of the database to
		/// which a connection will be made, and initializes it from the given model.
		/// The by-convention name is the full name (namespace + class name) of the derived context class.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(DbCompiledModel model)
			: base(model)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance using the given string as the name or connection string for the
		/// database to which a connection will be made, and initializes it from the given model.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(string nameOrConnectionString, DbCompiledModel model)
			: base(nameOrConnectionString, model)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(DbConnection existingConnection, bool contextOwnsConnection)
			: base(existingConnection, contextOwnsConnection)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance using the existing connection to connect to a database,
		/// and initializes it from the given model.
		/// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
		/// is <c>false</c>.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
			: base(existingConnection, model, contextOwnsConnection)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

		/// <summary>
		/// Constructs a new context instance around an existing ObjectContext.
		/// </summary>
		protected FixUtcDateTimeKindDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
			: base(objectContext, dbContextOwnsObjectContext)
		{
			((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, args) => FixDates(args);
		}

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

			modelBuilder.Conventions.Add(new ExtendedDateTimeColumnTypeConvention());
		}

		#endregion

		#region Private Methods

		private static readonly ConcurrentDictionary<Type, PropertyInfo[]> propsCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

		private static PropertyInfo[] GetDateProperties(Type type)
		{
			var list = new List<PropertyInfo>();
			foreach (PropertyInfo prop in type.GetProperties())
			{
				Type valType = prop.PropertyType;
				if (valType == typeof(DateTime) || valType == typeof(DateTime?))
				{
					list.Add(prop);
				}
			}
			list.TrimExcess();
			return list.ToArray();
		}

		private static void FixDates(ObjectMaterializedEventArgs evArg)
		{
			object entity = evArg.Entity;
			if (entity != null)
			{
				Type eType = entity.GetType();
				PropertyInfo[] propertyInfos = propsCache.GetOrAdd(eType, GetDateProperties);
				foreach (PropertyInfo propertyInfo in propertyInfos)
				{
					object curVal = propertyInfo.GetValue(entity);
					if (curVal != null)
					{
						DateTime newVal = DateTime.SpecifyKind((DateTime)curVal, DateTimeKind.Utc);
						propertyInfo.SetValue(entity, newVal);
					}
				}
			}
		}

		#endregion
	}
}