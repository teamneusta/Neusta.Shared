namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;

	public interface ILoggingTargetWithLayout : ILoggingTarget
	{
		[PublicAPI]
		string Layout { get; }
	}
}