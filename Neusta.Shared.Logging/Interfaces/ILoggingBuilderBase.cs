namespace Neusta.Shared.Logging
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface ILoggingBuilderBase : IFluentSyntax
	{
		/// <summary>
		/// Gets the logging builder configuration data.
		/// </summary>
		[PublicAPI]
		ILoggingConfiguration Configuration { get; }

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void UpdateConfiguration(ILoggingConfiguration configuration);

		/// <summary>
		/// Gets the logging adapter builders.
		/// </summary>
		[PublicAPI]
		IEnumerable<ILoggingAdapterBuilder> AdapterBuilders { get; }

		/// <summary>
		/// Registers the given logging adapter builder.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void RegisterAdapterBuilder(ILoggingAdapterBuilder adapterBuilder);

		/// <summary>
		/// Registers the given logging adapter builder.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		TBuilder RegisterAdapterBuilder<TBuilder>()
			where TBuilder : class, ILoggingAdapterBuilder, new();
	}
}