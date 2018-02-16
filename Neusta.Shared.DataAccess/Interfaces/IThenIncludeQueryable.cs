namespace Neusta.Shared.DataAccess
{
	using JetBrains.Annotations;

	public interface IThenIncludeQueryable<out TEntity, out TProperty>
		where TEntity : class
	{
		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		IDataContext DataContext { get; }
	}
}