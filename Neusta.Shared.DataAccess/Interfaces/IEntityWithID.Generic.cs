namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;

	[PublicAPI]
	public interface IEntityWithID<TID> : IEntity, IID<TID>
		where TID : IEquatable<TID>
	{
	}
}