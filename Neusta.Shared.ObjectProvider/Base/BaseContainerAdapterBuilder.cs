namespace Neusta.Shared.ObjectProvider.Base
{
	using CommonServiceLocator;

	public abstract class BaseContainerAdapterBuilder : IContainerAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseContainerAdapterBuilder"/> class.
		/// </summary>
		protected BaseContainerAdapterBuilder()
		{
		}

		#region Implementation of IContainerAdapterBuilder

		/// <summary>
		/// Builds the container adapter.
		/// </summary>
		public abstract IContainerAdapter Build(IContainerConfiguration configuration);

		/// <summary>
		/// Registers the given adapter as default container.
		/// </summary>
		public virtual void RegisterAsDefault(IContainerConfiguration configuration, IContainerAdapter adapter)
		{
			IObjectProvider objectProvider = adapter.ObjectProvider;

			ServiceLocator.SetLocatorProvider(() => objectProvider);
			ObjectProvider.SetProvider(objectProvider);
		}

		#endregion
	}
}