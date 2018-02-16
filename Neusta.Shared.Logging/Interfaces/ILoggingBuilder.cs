namespace Neusta.Shared.Logging
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	public interface ILoggingBuilder : ILoggingBuilderBase, IFluentSyntax
	{
		/// <summary>
		/// Builds the logging adapter using the previously registered builders.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ILoggingAdapter Build();

		/// <summary>
		/// Builds the logging adapter using the previously registered builders and registers
		/// it as the default adapter.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ILoggingAdapter BuildAsDefault();
	}
}
