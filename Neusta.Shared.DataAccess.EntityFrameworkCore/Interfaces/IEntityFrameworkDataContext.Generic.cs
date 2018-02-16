namespace Neusta.Shared.DataAccess.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IEntityFrameworkDataContext<TDataContext> : IEntityFrameworkDataContext, IDataContext<TDataContext>
		where TDataContext : IEntityFrameworkDataContext<TDataContext>
	{
		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		[PublicAPI]
		void AddBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Adds the given list of entities using a bulk insert operation.
		/// </summary>
		[PublicAPI]
		Task AddBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		[PublicAPI]
		void UpdateBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Updates the given list of entities using a bulk operation.
		/// </summary>
		[PublicAPI]
		Task UpdateBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		[PublicAPI]
		void DeleteBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Deletes the given list of entities using a bulk delete operation.
		/// </summary>
		[PublicAPI]
		Task DeleteBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		[PublicAPI]
		void MergeBulk<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		[PublicAPI]
		Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity;

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		[PublicAPI]
		void MergeBulk<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
			where TEntity : class, IEntity;

		/// <summary>
		/// Merges the given list of entities using a bulk merge operation.
		/// </summary>
		[PublicAPI]
		Task MergeBulkAsync<TEntity>(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> primaryKeyExpression)
			where TEntity : class, IEntity;
	}
}