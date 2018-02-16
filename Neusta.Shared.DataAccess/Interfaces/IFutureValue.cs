namespace Neusta.Shared.DataAccess
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	public interface IFutureValue<TEntity>
	{
		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		IDataContext DataContext { get; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		[PublicAPI]
		TEntity Value { get; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		[PublicAPI]
		Task<TEntity> ValueAsync();

		/// <summary>
		/// Gets the result directly.
		/// </summary>
		[PublicAPI]
		void GetResultDirectly();
	}
}