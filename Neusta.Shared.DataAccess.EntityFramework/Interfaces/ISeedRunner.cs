namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface ISeedRunner
	{
		/// <summary>
		/// Gets the initializer.
		/// </summary>
		[PublicAPI]
		IEnumerable<ISeed> Initializer { get; }

		/// <summary>
		/// Runs the database initializer.
		/// </summary>
		[PublicAPI]
		void Execute();
	}
}