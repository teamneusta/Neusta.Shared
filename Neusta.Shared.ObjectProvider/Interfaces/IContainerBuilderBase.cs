namespace Neusta.Shared.ObjectProvider
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface IContainerBuilderBase : IFluentSyntax
	{
		/// <summary>
		/// Gets the container builder configuration data.
		/// </summary>
		[PublicAPI]
		IContainerConfiguration Configuration { get; }

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void UpdateConfiguration(IContainerConfiguration configuration);

		/// <summary>
		/// Gets the container adapter builder.
		/// </summary>
		[PublicAPI]
		IContainerAdapterBuilder AdapterBuilder { get; }

		/// <summary>
		/// Registers the given container adapter builder.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void RegisterAdapterBuilder(IContainerAdapterBuilder adapterBuilder);

		/// <summary>
		/// Registers the given container adapter builder.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		TBuilder RegisterAdapterBuilder<TBuilder>()
			where TBuilder : class, IContainerAdapterBuilder, new();
	}
}