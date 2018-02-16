namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal interface IThenIncludeImplemenation<TEntity>
	{
		IQueryable<TEntity> Queryable { get; }
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal interface IThenIncludeImplemenation<TEntity, TProperty> : IThenIncludeImplemenation<TEntity>
		where TEntity : class
	{
		Expression<Func<TEntity, TProperty>> Expression { get; }
	}
}