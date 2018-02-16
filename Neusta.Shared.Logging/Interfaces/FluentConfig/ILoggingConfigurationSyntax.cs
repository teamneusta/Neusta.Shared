namespace Neusta.Shared.Logging
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ILoggingConfigurationSyntax : IFluentSyntax
	{
		/// <summary>
		/// Configures the minimum logging level.
		/// </summary>
		[PublicAPI]
		ILogLevelSyntax<ILoggingConfigurationSyntax> MinimumLevel { get; }

		/// <summary>
		/// Configures the default logging targets.
		/// </summary>
		[PublicAPI]
		IToSyntax To { get; }
	}
}