namespace Neusta.Shared.ObjectProvider
{
	using System.Reflection;
	using JetBrains.Annotations;

	public interface IContainerConfiguration
	{
		/// <summary>
		/// Gets the application <see cref="Assembly" />.
		/// </summary>
		[PublicAPI]
		Assembly ApplicationAssembly { get; }

		/// <summary>
		/// Gets or sets a value indicating whether unknown types should be resolved automatically.
		/// </summary>
		[PublicAPI]
		bool AutoResolveUnknownTypes { get; set; }

		/// <summary>
		/// Gets the service descriptors.
		/// </summary>
		[PublicAPI]
		IServiceDescriptorCollection ServiceDescriptors { get; }
	}
}