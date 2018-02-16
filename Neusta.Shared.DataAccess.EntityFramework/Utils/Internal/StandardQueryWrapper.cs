namespace Neusta.Shared.DataAccess.EntityFramework.Utils.Internal
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	internal sealed class StandardQueryWrapper<TEntity> : IFutureQuery<TEntity>
	{
		private readonly IDataContext dataContext;
		private readonly IQueryable<TEntity> query;

		/// <summary>
		/// Initializes a new instance of the <see cref="StandardQueryWrapper{TEntity}"/> class.
		/// </summary>
		public StandardQueryWrapper(IDataContext dataContext, IQueryable<TEntity> query)
		{
			this.dataContext = dataContext;
			this.query = query;
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
			return this.query.ToListAsync(this.dataContext);
		}

		/// <summary>
		/// Creates a array from an futured <see cref="T:System.Linq.IQueryable`1" /> by enumerating it asynchronously.
		/// </summary>
		public Task<TEntity[]> ToArrayAsync()
		{
			return this.query.ToArrayAsync(this.dataContext);
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
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.query).GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable<out TEntity>

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<TEntity> GetEnumerator()
		{
			return this.query.GetEnumerator();
		}

		#endregion
	}
}