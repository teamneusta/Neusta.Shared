namespace Neusta.Shared.DataAccess.EntityFramework.Utils.Internal
{
	using System.Linq;
	using System.Threading.Tasks;

	internal sealed class StandardValueWrapper<TEntity> : IFutureValue<TEntity>
	{
		private readonly IDataContext dataContext;
		private readonly IQueryable<TEntity> query;

		/// <summary>
		/// Initializes a new instance of the <see cref="StandardValueWrapper{TEntity}"/> class.
		/// </summary>
		public StandardValueWrapper(IDataContext dataContext, IQueryable<TEntity> query)
		{
			this.dataContext = dataContext;
			this.query = query;
		}

		#region Implementation of IFutureValue<TEntity>

		/// <summary>
		/// Gets the data context.
		/// </summary>
		public IDataContext DataContext => this.dataContext;

		/// <summary>
		/// Gets the value.
		/// </summary>
		public TEntity Value => this.query.FirstOrDefault();

		/// <summary>
		/// Gets the value.
		/// </summary>
		public Task<TEntity> ValueAsync()
		{
			return this.query.FirstOrDefaultAsync(this.dataContext);
		}

		/// <summary>
		/// Gets the result directly.
		/// </summary>
		public void GetResultDirectly()
		{
		}

		#endregion

		#region Implicit type conversions

		public static implicit operator TEntity(StandardValueWrapper<TEntity> futureValue)
		{
			return futureValue.Value;
		}

		#endregion
	}
}