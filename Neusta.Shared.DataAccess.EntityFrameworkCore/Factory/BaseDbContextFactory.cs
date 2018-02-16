namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Factory
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Internal;
	using Neusta.Shared.Core.Utils;

	[PublicAPI]
	public abstract class BaseDbContextFactory<TDbContextFactory, TDbContext> : Singleton<TDbContextFactory>, IDbContextFactory<TDbContext>
		where TDbContextFactory : BaseDbContextFactory<TDbContextFactory, TDbContext>
		where TDbContext : DbContext
	{
		private readonly Func<TDbContext> activator;
		private readonly DbContextOptions options;
		private readonly DbContextPool<TDbContext> pool;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDbContextFactory{TDbContextFactory, TDbContext}"/> class.
		/// </summary>
		protected BaseDbContextFactory()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDbContextFactory{TDataContextFactory,TDbContext}" /> class.
		/// </summary>
		protected BaseDbContextFactory(DbContextOptions options)
		{
			// ReSharper disable VirtualMemberCallInConstructor
			// Options
			DbContextOptionsBuilder builder;
			if (options != null)
			{
				if (options is DbContextOptions<TDbContext> typedOptions)
				{
					builder = new DbContextOptionsBuilder<TDbContext>(typedOptions);
				}
				else
				{
					builder = new DbContextOptionsBuilder(options);
				}
			}
			else
			{
				builder = new DbContextOptionsBuilder<TDbContext>();
			}
			this.BuildOptions(builder);
			this.options = builder.Options;

			// Pool
			this.activator = CreateDbContextActivator(this.options);
			this.pool = new DbContextPool<TDbContext>(this.options);

			this.InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
			// ReSharper restore VirtualMemberCallInConstructor
		}

		#region Implementation of IDbContextFactory<TDbContext>

		/// <summary>
		/// Gets the options.
		/// </summary>
		public DbContextOptions Options
		{
			[DebuggerStepThrough]
			get { return this.options; }
		}

		/// <summary>
		/// Creates a <see cref="TDbContext"/>.
		/// </summary>
		public virtual TDbContext CreateDbContext()
		{
			return this.activator();
		}

		/// <summary>
		/// Rents a <see cref="TDbContext" /> from the factory.
		/// </summary>
		public virtual TDbContext RentDbContext()
		{
			return this.pool.Rent();
		}

		/// <summary>
		/// Returns the given a <see cref="TDbContext" /> to the factory.
		/// </summary>
		public void ReturnDbContext(TDbContext dbContext)
		{
			this.pool.Return(dbContext);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Builds the options.
		/// </summary>
		protected virtual void BuildOptions(DbContextOptionsBuilder optionsBuilder)
		{
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected virtual async Task InitializeAsync()
		{
			using (TDbContext context = this.CreateDbContext())
			{
				await context.Database.MigrateAsync().ConfigureAwait(false);
				await this.InitializeAsync(context).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		protected virtual Task InitializeAsync(TDbContext context)
		{
			return Task.CompletedTask;
		}

		#endregion

		#region Private Methods

		private static Func<TDbContext> CreateDbContextActivator(DbContextOptions options)
		{
			ConstructorInfo[] array = typeof(TDbContext).GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic && c.IsPublic).ToArray();
			if (array.Length == 1)
			{
				ParameterInfo[] parameters = array[0].GetParameters();
				if (parameters.Length == 1 && (parameters[0].ParameterType == typeof(DbContextOptions) || parameters[0].ParameterType == typeof(DbContextOptions<TDbContext>)))
				{
					return Expression.Lambda<Func<TDbContext>>(Expression.New(array[0], new []
					{
						Expression.Constant(options)
					}), Array.Empty<ParameterExpression>()).Compile();
				}
			}
			return null;
		}

		#endregion
	}
}