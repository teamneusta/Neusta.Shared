namespace Neusta.Shared.ObjectProvider
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface IContainerAdapterBuilder
	{
		/// <summary>
		/// Builds the container adapter.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IContainerAdapter Build(IContainerConfiguration configuration);

		/// <summary>
		/// Registers the given adapter as default container.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void RegisterAsDefault(IContainerConfiguration configuration, IContainerAdapter adapter);
	}
}