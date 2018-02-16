namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;

	public interface ILogger<T> : ILogger
	{
		/// <summary>
		/// Gets the generic class.
		/// </summary>
		[PublicAPI]
		Type GenericClass { get; }
	}
}