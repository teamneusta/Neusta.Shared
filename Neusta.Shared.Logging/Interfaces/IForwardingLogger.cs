namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;

	public interface IForwardingLogger : ILogger
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		[PublicAPI]
		ILogger TargetLogger { get; set; }
	}
}