namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;

	public interface ILogSyntaxResult : ILogSyntax, IFluentSyntax
	{
		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		[PublicAPI]
		void Write();

		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		[PublicAPI]
		void WriteIf(bool condition);

		/// <summary>
		/// Writes the log event to the underlying logger.
		/// </summary>
		[PublicAPI]
		void WriteIf(Func<bool> condition);
	}
}