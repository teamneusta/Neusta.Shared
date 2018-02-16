namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IFilterRule
	{
		/// <summary>
		/// Gets the adapter name this rule applies to.
		/// </summary>
		string AdapterName { get; }

		/// <summary>
		/// Gets the logger name this rule applies to.
		/// </summary>
		string LoggerName { get; }

		/// <summary>
		/// Gets the minimum <see cref="LogLevel"/> of messages.
		/// </summary>
		LogLevel LogLevel { get; }

		/// <summary>
		/// Gets the filter delegate that would be applied to messages that passed the <see cref="LogLevel"/>.
		/// </summary>
		Func<string, string, LogLevel, bool> Filter { get; }

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		[PublicAPI]
		object Tag { get; set; }
	}
}