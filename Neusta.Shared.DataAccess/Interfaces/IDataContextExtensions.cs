namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IDataContextExtensions : IDataContext
	{
		/// <summary>
		/// Applys the specified <see cref="QueryOptions"/> to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> ApplyOptions<TEntity>(IQueryable<TEntity> source, QueryOptions options)
			where TEntity : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> source, string path)
			where TEntity : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr)
			where TEntity : class
			where TProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> expr)
			where TEntity : class
			where TProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>,
				IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, IEnumerable<TProperty>>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, IEnumerable<TProperty>>, IThenIncludeQueryable<TEntity, TThenProperty>
			>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, IEnumerable<TThenProperty>>
			>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IQueryable<TEntity> Include<TEntity, TProperty, TThenProperty>(
			IQueryable<TEntity> source,
			Expression<Func<TEntity, TProperty>> expr,
			Expression<Func<IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeQueryable<TEntity, TThenProperty>>> then)
			where TEntity : class
			where TProperty : class
			where TThenProperty : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IThenIncludeQueryable<TEntity, TThenProperty> ThenInclude<TEntity, TPreviousProperty, TThenProperty>(
			IThenIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
			Expression<Func<TPreviousProperty, TThenProperty>> expr)
			where TEntity : class;

		/// <summary>
		/// Includes the specified property to the query.
		/// </summary>
		[PublicAPI]
		IThenIncludeQueryable<TEntity, TThenProperty> ThenInclude<TEntity, TPreviousProperty, TThenProperty>(
			IThenIncludeQueryable<TEntity, TPreviousProperty> source,
			Expression<Func<TPreviousProperty, TThenProperty>> expr)
			where TEntity : class;

		/// <summary>
		/// Creates a <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		Task<List<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Creates a array from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		Task<TEntity[]> ToArrayAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		Task<TEntity> FirstOrDefaultAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Asynchronously returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		Task<TEntity> LastOrDefaultAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		[PublicAPI]
		Task<TEntity> SingleOrDefaultAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Asynchronously determines whether a sequence contains any elements.
		/// </summary>
		[PublicAPI]
		Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[PublicAPI]
		IFutureQuery<TEntity> Future<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Futures the specified query.
		/// </summary>
		[PublicAPI]
		IFutureValue<TEntity> FutureValue<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" />.
		/// </summary>
		[PublicAPI]
		List<TEntity> ToDetachedList<TEntity>(IQueryable<TEntity> source);

		/// <summary>
		/// Creates a detached <see cref="List{TEntity}" /> from an <see cref="IQueryable{TEntity}" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		Task<List<TEntity>> ToDetachedListAsync<TEntity>(IQueryable<TEntity> source);
	}
}