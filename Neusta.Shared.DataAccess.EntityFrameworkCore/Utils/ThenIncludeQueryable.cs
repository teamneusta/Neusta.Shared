namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Utils
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	internal sealed class ThenIncludeQueryable<TEntity, TProperty> : IThenIncludeQueryable<TEntity, TProperty>, IThenIncludeImplemenation<TEntity, TProperty>
		where TEntity : class
	{
		private readonly IDataContext dataContext;
		private readonly IQueryable<TEntity> entityQueryable;
		private readonly Expression<Func<TEntity, TProperty>> expr;

		public ThenIncludeQueryable(IDataContext dataContext, IQueryable<TEntity> entityQueryable, Expression<Func<TEntity, TProperty>> expr)
		{
			this.dataContext = dataContext;
			this.entityQueryable = entityQueryable;
			this.expr = expr;
		}

		#region Implementation of IThenIncludeQueryable

		public IDataContext DataContext => this.dataContext;

		#endregion

		#region Implementation of IThenIncludeImplemenation

		public Expression<Func<TEntity, TProperty>> Expression => this.expr;

		public IQueryable<TEntity> Queryable => this.entityQueryable;

		#endregion
	}
}