namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;

	public interface IObjectProviderScope : IObjectProvider, IDisposable
	{
		/// <summary>
		/// Gets the scope name.
		/// </summary>
		[PublicAPI]
		string Name { get; }

		/// <summary>
		/// Gets the parent provider.
		/// </summary>
		[PublicAPI]
		IObjectProvider ParentProvider { get; }
	}
}