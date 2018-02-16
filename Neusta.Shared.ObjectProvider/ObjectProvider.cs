namespace Neusta.Shared.ObjectProvider
{
	using System.ComponentModel;
	using System.Diagnostics;
	using CommonServiceLocator;
	using JetBrains.Annotations;

	public static class ObjectProvider
	{
		private static IObjectProvider defaultInstance;

		/// <summary>
		/// Initializes the ObjectProvider and registers it with the ServiceLocator
		/// </summary>
		static ObjectProvider()
		{
			RegisterWithServiceLocator();
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		[PublicAPI]
		public static IObjectProvider Default
		{
			[DebuggerStepThrough]
			get { return defaultInstance; }
		}

		/// <summary>
		/// Gets a value indicating whether a provider is set.
		/// </summary>
		[PublicAPI]
		public static bool IsProviderSet
		{
			[DebuggerStepThrough]
			get { return defaultInstance != null; }
		}

		/// <summary>
		/// Registers the specified object provider implementation
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetProvider(IObjectProvider provider)
		{
			if (provider == null)
			{
				throw new ObjectProviderException(nameof(provider) + " cannot be null.");
			}
			ObjectProvider.defaultInstance = provider;
		}

		/// <summary>
		/// Registers the specified object provider implementation
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetProvider(IObjectProviderProvider provider)
		{
			if (provider == null)
			{
				throw new ObjectProviderException(nameof(provider) + " cannot be null.");
			}
			SetProvider(provider.ObjectProvider);
		}

		/// <summary>
		/// Registers the object provider the with service locator.
		/// </summary>
		[PublicAPI]
		public static void RegisterWithServiceLocator()
		{
			ServiceLocator.SetLocatorProvider(() => defaultInstance);
		}
	}
}