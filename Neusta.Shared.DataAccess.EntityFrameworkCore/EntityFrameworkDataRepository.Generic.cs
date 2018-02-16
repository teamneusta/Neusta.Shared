namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.Repository;

	[PublicAPI]
	public abstract class EntityFrameworkDataRepository<TDataContext> : BaseDataRepository<TDataContext>, IEntityFrameworkDataRepository<TDataContext>
		where TDataContext : IEntityFrameworkDataContext<TDataContext>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFrameworkDataRepository{TDataContext}"/> class.
		/// </summary>
		protected EntityFrameworkDataRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}
	}
}