namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	public interface IObjectFactory
	{
		/// <summary>
		/// Creates an instance using specified provider.
		/// </summary>
		[PublicAPI]
		object Create(IObjectProvider provider);
	}
}