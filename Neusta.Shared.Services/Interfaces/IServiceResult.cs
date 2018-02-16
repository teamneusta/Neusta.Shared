namespace Neusta.Shared.Services
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface IServiceResult
	{
		/// <summary>
		/// Gets the errors.
		/// </summary>
		[PublicAPI]
		IReadOnlyDictionary<string, IReadOnlyCollection<IServiceResultError>> Errors { get; }

		/// <summary>
		/// Gets the result code.
		/// </summary>
		[PublicAPI]
		Nullable<int> ResultCode { get; }
	}
}