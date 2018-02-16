namespace Neusta.Shared.DataAccess
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IInitializeEntityLists
	{
		/// <summary>
		/// Initializes the sub-lists of this instance.
		/// </summary>
		[PublicAPI]
		void InitializeLists();
	}
}