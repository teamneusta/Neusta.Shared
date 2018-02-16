// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using CommonServiceLocator;
	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	public static class IContainerAdapterExtensions
	{
		/// <summary>
		/// Registers this container with the <see cref="ServiceLocator" />.
		/// </summary>
		[PublicAPI]
		public static IContainerAdapter RegisterWithServiceLocator(this IContainerAdapter adapter)
		{
			IObjectProvider objectProvider = adapter.ObjectProvider;
			ServiceLocator.SetLocatorProvider(() => objectProvider);

			return adapter;
		}

		/// <summary>
		/// Registers this container with the <see cref="ObjectProvider" />.
		/// </summary>
		[PublicAPI]
		public static IContainerAdapter RegisterWithObjectProvider(this IContainerAdapter adapter)
		{
			IObjectProvider objectProvider = adapter.ObjectProvider;
			ObjectProvider.SetProvider(objectProvider);

			return adapter;
		}
	}
}