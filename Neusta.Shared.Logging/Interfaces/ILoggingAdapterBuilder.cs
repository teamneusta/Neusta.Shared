namespace Neusta.Shared.Logging
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface ILoggingAdapterBuilder : IFluentSyntax
	{
		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		ILoggingAdapter Build(ILoggingConfiguration configuration);

		/// <summary>
		/// Registers the given adapter as default logger.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void RegisterAsDefault(ILoggingConfiguration configuration, ILoggingAdapter adapter);
	}
}