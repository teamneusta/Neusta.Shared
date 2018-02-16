// ReSharper disable once CheckNamespace

namespace System.Linq
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess;

	// ReSharper disable once InconsistentNaming
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Applys the specified <see cref="QueryOptions"/> to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> ApplyOptions<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			QueryOptions options)
			where TEntity : class
		{
			return dataContext.Extensions.ApplyOptions(source, options) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			string path)
			where TEntity : class
		{
			return dataContext.Extensions.Include(source, path) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr)
			where TEntity : class
			where TProperty : class
		{
			return dataContext.Extensions.Include(source, expr) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, TProperty>> expr)
			where TEntity : class
			where TProperty : class
		{
			return dataContext.Extensions.Include(source, expr) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>,
			IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return dataContext.Extensions.Include(source, expr, then) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return dataContext.Extensions.Include(source, expr, then) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return dataContext.Extensions.Include(source, expr, then) ?? source;
		}

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class
		{
			return dataContext.Extensions.Include(source, expr, then) ?? source;
		}

		/// <summary>
		/// Creates a <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<List<TEntity>> ToListAsync<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext)
		{
			return dataContext.Extensions.ToListAsync(source) ?? Task.FromResult(source.ToList());
		}

		/// <summary>
		/// Creates an array of <see cref="TEntity" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<TEntity[]> ToArrayAsync<TEntity>(this IQueryable<TEntity> source, IDataContextOwner dataContext)
		{
			return dataContext.Extensions.ToArrayAsync(source) ?? Task.FromResult(source.ToArray());
		}

		/// <summary>
		/// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<TEntity> FirstOrDefaultAsync<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext)
		{
			return dataContext.Extensions.FirstOrDefaultAsync(source) ?? Task.FromResult(source.FirstOrDefault());
		}

		/// <summary>
		/// Asynchronously returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<TEntity> LastOrDefaultAsync<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext)
		{
			return dataContext.Extensions.LastOrDefaultAsync(source) ?? Task.FromResult(source.LastOrDefault());
		}

		/// <summary>
		/// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<TEntity> SingleOrDefaultAsync<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext)
		{
			return dataContext.Extensions.SingleOrDefaultAsync(source) ?? Task.FromResult(source.SingleOrDefault());
		}

		/// <summary>
		/// Asynchronously determines whether a sequence contains any elements.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<bool> AnyAsync<TEntity>(this IQueryable<TEntity> source, IDataContextOwner dataContext)
		{
			return dataContext.Extensions.AnyAsync(source) ?? Task.FromResult(source.Any());
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IFutureQuery<TEntity> Future<TEntity>(this IQueryable<TEntity> source, IDataContextOwner dataContext)
		{
			return dataContext.Extensions.Future(source);
		}

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IFutureValue<TEntity> FutureValue<TEntity>(
			this IQueryable<TEntity> source,
			IDataContextOwner dataContext)
		{
			return dataContext.Extensions.FutureValue(source);
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" />.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<TEntity> ToDetachedList<TEntity>(this IQueryable<TEntity> source, IDataContextOwner dataContext)
			where TEntity : class, IEntity
		{
			return dataContext.Extensions.ToDetachedList(source) ?? source.ToList();
		}

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task<List<TEntity>> ToDetachedListAsync<TEntity>(this IQueryable<TEntity> source, IDataContextOwner dataContext)
			where TEntity : class, IEntity
		{
			return dataContext.Extensions.ToDetachedListAsync(source) ?? Task.FromResult(source.ToList());
		}
	}
}