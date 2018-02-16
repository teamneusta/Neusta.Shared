namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface ISeed
	{
		/// <summary>
		/// Initializes the database.
		/// </summary>
		[PublicAPI]
		Task InitializeAsync();
	}
}