namespace Neusta.Shared.Core
{
	using System;
	using JetBrains.Annotations;

	public interface IID<TID> : IID, IEquatable<TID>
		where TID : IEquatable<TID>
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		[PublicAPI]
		new TID ID { get; set; }
	}
}