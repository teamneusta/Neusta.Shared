namespace Neusta.Shared.DataAccess
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IEntityWithID : IEntityWithID<long>
	{
	}
}