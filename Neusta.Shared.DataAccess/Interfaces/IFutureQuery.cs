namespace Neusta.Shared.DataAccess
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IFutureQuery<TEntity> : IEnumerable<TEntity>, IEnumerable
	{
		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		IDataContext DataContext { get; }

		/// <summary>
		/// Creates a <see cref="T:System.Collections.Generic.List`1" /> from an futured <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		Task<List<TEntity>> ToListAsync();

		/// <summary>
		/// Creates a array from an futured <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		[PublicAPI]
		Task<TEntity[]> ToArrayAsync();

		/// <summary>
		/// Returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		Task<TEntity> FirstOrDefaultAsync();

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		[PublicAPI]
		Task<TEntity> SingleOrDefaultAsync();

		/// <summary>
		/// Returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		[PublicAPI]
		Task<TEntity> LastOrDefaultAsync();

		/// <summary>
		/// Gets the result directly.
		/// </summary>
		[PublicAPI]
		void GetResultDirectly();
	}
}