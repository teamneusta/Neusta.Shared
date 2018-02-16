namespace Neusta.Shared.DataAccess.Utils
{
	using System;
	using JetBrains.Annotations;

	// ReSharper disable once MismatchedFileName
	public static class EntityActivator<TEntity, TID>
		where TEntity : class, IEntityWithID<TID>
		where TID : struct, IEquatable<TID>
	{
		/// <summary>
		/// Creates a new instance of the given entity type.
		/// </summary>
		[PublicAPI]
		public static TEntity CreateInstance(TID id)
		{
			TEntity entity = EntityActivator<TEntity>.Create();
			entity.ID = id;
			return entity;
		}
	}
}