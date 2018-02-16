namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	[PublicAPI]
	public enum ImplementationSource
	{
		None,
		Type,
		Instance,
		Factory,
		FactoryWithProvider,
		FactoryInvalid,
	}
}