namespace Neusta.Shared.Services
{
	using System;
	using JetBrains.Annotations;

	public interface IServiceResultError
	{
		/// <summary>
		/// Gets the message.
		/// </summary>
		[PublicAPI]
		string Message { get; }

		/// <summary>
		/// Gets the exception.
		/// </summary>
		[PublicAPI]
		Exception Exception { get; }
	}
}