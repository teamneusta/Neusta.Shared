namespace Neusta.Shared.DataAccess.Repository
{
	public abstract class BaseEntityRepository : BaseDataRepository, IEntityRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseEntityRepository"/> class.
		/// </summary>
		protected BaseEntityRepository(IDataContext dataContext)
			: base(dataContext)
		{
		}

		#region Implementation of IEntityRepository

		/// <summary>
		/// Inserts the specified entity.
		/// </summary>
		public void Insert(IEntity entity)
		{
			this.DataContext.Add(entity);
		}

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		public void Delete(IEntity entity)
		{
			this.DataContext.Delete(entity);
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		public void Update(IEntity entity)
		{
			this.DataContext.Update(entity);
		}

		#endregion
	}
}