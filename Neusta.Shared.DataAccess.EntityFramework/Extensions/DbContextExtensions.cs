// ReSharper disable once CheckNamespace

namespace System.Data.Entity
{
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.DataAccess;

	public static class DbContextExtensions
	{
		[PublicAPI]
		public static void AddOrUpdate<TEntity>(this DbContext dbContext, TEntity entity)
			where TEntity : class, IEntityWithID
		{
			dbContext.AddOrUpdate<TEntity, long>(entity);
		}

		[PublicAPI]
		public static void AddOrUpdate<TEntity, TID>(this DbContext dbContext, TEntity entity)
			where TEntity : class, IEntityWithID<TID>
			where TID : struct, IEquatable<TID>
		{
			Guard.ArgumentNotNull(entity, nameof(entity));

			DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
			if (entry.State == EntityState.Detached)
			{
				TID entityID = entity.ID;
				DbSet<TEntity> set = dbContext.Set<TEntity>();
				if (entityID.Equals(default(TID)))
				{
					set.Add(entity);
				}
				else
				{
					TEntity attachedEntity = set.Local.SingleOrDefault(e => entityID.Equals(e.ID));
					if (attachedEntity != null)
					{
						DbEntityEntry<TEntity> attachedEntry = dbContext.Entry(attachedEntity);
						attachedEntry.CurrentValues.SetValues(entity);
					}
					else
					{
						entry.State = EntityState.Modified;
					}
				}
			}
		}
	}
}