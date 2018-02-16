namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	public interface IContainerAdapter : IObjectProviderProvider
	{
		/// <summary>
		/// Gets the configuration.
		/// </summary>
		[PublicAPI]
		IContainerConfiguration Configuration { get; }
	}
}