namespace Neusta.Shared.ObjectProvider
{
	using System;
	using CommonServiceLocator;
	using JetBrains.Annotations;

	public interface IObjectProvider : IServiceLocator, IServiceProvider
	{
		/// <summary>
		/// Checks if the provider has a binding for the specified service type.
		/// </summary>
		[PublicAPI]
		bool Has(Type serviceType);

		/// <summary>
		/// Checks if the provider has a binding for the specified service type.
		/// </summary>
		[PublicAPI]
		bool Has<TService>();

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Throws an exception if the specified type is not available.
		/// </summary>
		[PublicAPI]
		object GetInstance(Type serviceType, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Throws an exception if the specified type is not available.
		/// </summary>
		[PublicAPI]
		object GetInstance(Type serviceType, string key, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Throws an exception if the specified type is not available.
		/// </summary>
		[PublicAPI]
		TService GetInstance<TService>(object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Throws an exception if the specified type is not available.
		/// </summary>
		[PublicAPI]
		TService GetInstance<TService>(string key, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[PublicAPI]
		object QueryInstance(Type serviceType, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[PublicAPI]
		object QueryInstance(Type serviceType, string key, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[PublicAPI]
		TService QueryInstance<TService>(object[] dependencyOverrides = null);

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[PublicAPI]
		TService QueryInstance<TService>(string key, object[] dependencyOverrides = null);

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		[PublicAPI]
		IObjectProviderScope CreateScope(string name = null);
	}
}