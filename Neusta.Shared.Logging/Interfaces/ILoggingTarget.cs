namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;

	public interface ILoggingTarget
	{
		[PublicAPI]
		string Name { get; }
	}
}