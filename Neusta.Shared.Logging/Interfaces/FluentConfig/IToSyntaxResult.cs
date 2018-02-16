namespace Neusta.Shared.Logging
{
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IToSyntaxResult : ILoggingConfigurationSyntax, IFluentSyntax
	{
	}
}