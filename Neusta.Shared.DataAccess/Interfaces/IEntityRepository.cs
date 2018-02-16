namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;

	public interface IEntityRepository : IDataRepository
	{
		/// <summary>
		/// Inserts the specified entity.
		/// </summary>
		[PublicAPI]
		void Insert(IEntity entity);

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		[PublicAPI]
		void Delete(IEntity entity);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		[PublicAPI]
		void Update(IEntity entity);
	}
}