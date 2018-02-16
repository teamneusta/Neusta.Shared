namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;

	public interface ILoggingRule
	{
		/// <summary>
		/// Gets the minimum level.
		/// </summary>
		[PublicAPI]
		LogLevel MinLevel { get; }

		/// <summary>
		/// Gets the maximum level.
		/// </summary>
		[PublicAPI]
		LogLevel MaxLevel { get; }

		/// <summary>
		/// Gets the name of the target.
		/// </summary>
		[PublicAPI]
		string TargetName { get; }

		/// <summary>
		/// Gets the logger name pattern.
		/// </summary>
		[PublicAPI]
		string LoggerNamePattern { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="ILoggingRule"/> is a final rule.
		/// </summary>
		[PublicAPI]
		bool Final { get; }

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		[PublicAPI]
		object Tag { get; set; }
	}
}