namespace Neusta.Shared.Core
{
	using JetBrains.Annotations;

	public interface IID
	{
		/// <summary>
		/// Gets the unique identifier.
		/// </summary>
		[PublicAPI]
		object ID { get; }

		/// <summary>
		/// Checks if the ID of the other object is the same as our ID.
		/// </summary>
		[PublicAPI]
		bool IsIDEqual(object other);
	}
}