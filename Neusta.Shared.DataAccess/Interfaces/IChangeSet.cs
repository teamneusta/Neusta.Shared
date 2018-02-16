namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IChangeSet
	{
		/// <summary>
		/// Gets the context.
		/// </summary>
		[PublicAPI]
		Type Context { get; }

		/// <summary>
		/// Gets the changes.
		/// </summary>
		[PublicAPI]
		IEnumerable<IEntityChange> Changes { get; }
	}
}