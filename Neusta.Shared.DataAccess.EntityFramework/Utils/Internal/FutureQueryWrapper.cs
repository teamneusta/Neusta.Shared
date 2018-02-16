namespace Neusta.Shared.DataAccess.EntityFramework.Utils.Internal
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Z.EntityFramework.Plus;

	internal sealed class FutureQueryWrapper<TEntity> : IFutureQuery<TEntity>
	{
		private readonly IDataContext dataContext;
		private readonly QueryFutureEnumerable<TEntity> futureQuery;

		/// <summary>
		/// Initializes a new instance of the <see cref="FutureValueWrapper{TEntity}"/> class.
		/// </summary>
		public FutureQueryWrapper(IDataContext dataContext, QueryFutureEnumerable<TEntity> futureQuery)
		{
			this.dataContext = dataContext;
			this.futureQuery = futureQuery;
		}

		#region Implementation of IFutureQuery<TEntity>

		/// <summary>
		/// Gets the data context.
		/// </summary>
		public IDataContext DataContext => this.dataContext;

		/// <summary>
		/// Creates a <see cref="T:System.Collections.Generic.List`1" /> from an futured <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		public Task<List<TEntity>> ToListAsync()
		{
			return this.futureQuery.ToListAsync();
		}

		/// <summary>
		/// Creates a array from an futured <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		public Task<TEntity[]> ToArrayAsync()
		{
			return this.futureQuery.ToArrayAsync();
		}

		/// <summary>
		/// Returns the first element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		public async Task<TEntity> FirstOrDefaultAsync()
		{
			return (await this.ToListAsync().ConfigureAwait(false)).FirstOrDefault();
		}

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty;
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		public async Task<TEntity> SingleOrDefaultAsync()
		{
			return (await this.ToListAsync().ConfigureAwait(false)).SingleOrDefault();
		}

		/// <summary>
		/// Returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		public async Task<TEntity> LastOrDefaultAsync()
		{
			return (await this.ToListAsync().ConfigureAwait(false)).LastOrDefault();
		}

		/// <summary>
		/// Gets the result directly.
		/// </summary>
		public void GetResultDirectly()
		{
			this.futureQuery.GetResultDirectly();
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.futureQuery).GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable<out TEntity>

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<TEntity> GetEnumerator()
		{
			return this.futureQuery.GetEnumerator();
		}

		#endregion
	}
}