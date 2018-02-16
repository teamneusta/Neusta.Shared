// ReSharper disable once CheckNamespace
namespace Neusta.Shared.DataAccess
{
	using JetBrains.Annotations;

	public interface IUnitOfWorkOwner
	{
		/// <summary>
		///  Gets the current unit of work.
		/// </summary>
		[PublicAPI]
		IUnitOfWork UnitOfWork { get; }
	}
}