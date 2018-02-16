namespace Neusta.Shared.DataAccess.EntityFramework.Utils.Internal
{
	using System;
	using System.Threading.Tasks;
	using Z.EntityFramework.Plus;

	internal sealed class FutureValueWrapper<TEntity> : IFutureValue<TEntity>
	{
		private readonly IDataContext dataContext;
		private readonly QueryFutureValue<TEntity> futureValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="FutureValueWrapper{TEntity}"/> class.
		/// </summary>
		public FutureValueWrapper(IDataContext dataContext, QueryFutureValue<TEntity> futureValue)
		{
			this.dataContext = dataContext;
			this.futureValue = futureValue;
		}

		#region Implementation of IFutureValue<TEntity>

		/// <summary>
		/// Gets the data context.
		/// </summary>
		public IDataContext DataContext => this.dataContext;

		/// <summary>
		/// Gets the value.
		/// </summary>
		public TEntity Value => this.futureValue.Value;

		/// <summary>
		/// Gets the value.
		/// </summary>
		public Task<TEntity> ValueAsync()
		{
			return this.futureValue.ValueAsync();
		}

		/// <summary>
		/// Gets the result directly.
		/// </summary>
		public void GetResultDirectly()
		{
			this.futureValue.GetResultDirectly();
		}

		#endregion

		#region Implicit type conversions

		public static implicit operator TEntity(FutureValueWrapper<TEntity> futureValue)
		{
			return futureValue.Value;
		}

		#endregion
	}
}