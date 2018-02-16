namespace Neusta.Shared.DataAccess
{
	using System;
	using JetBrains.Annotations;

	public interface IDataContextProvider
	{
		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		IDataContext GetDataContext(Type dataContextType);

		/// <summary>
		/// Gets the data context of the specified type.
		/// </summary>
		[PublicAPI]
		TDataContext GetDataContext<TDataContext>()
			where TDataContext : IDataContext;
	}
}