namespace Neusta.Shared.ObjectProvider
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ILifetimeSelector : IServiceTypeSelector
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IImplementationTypeSelector InternalWithLifetime(ServiceLifetime lifetime);
	}
}