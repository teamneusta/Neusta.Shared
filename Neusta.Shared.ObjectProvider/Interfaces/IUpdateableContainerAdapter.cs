namespace Neusta.Shared.ObjectProvider
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface IUpdateableContainerAdapter : IContainerAdapter
	{
		/// <summary>
		/// Gets the service descriptors.
		/// </summary>
		[PublicAPI]
		IEnumerable<IServiceDescriptor> ServiceDescriptors { get; }

		/// <summary>
		/// Registers the service descriptor.
		/// </summary>
		[PublicAPI]
		void RegisterServiceDescriptor(IServiceDescriptor serviceDescriptor);

		/// <summary>
		/// Unregisters the service descriptor.
		/// </summary>
		[PublicAPI]
		void UnregisterServiceDescriptor(IServiceDescriptor serviceDescriptor);

		/// <summary>
		/// Begins a mass update.
		/// </summary>
		[PublicAPI]
		void BeginUpdate();

		/// <summary>
		/// Ends a mass update.
		/// </summary>
		[PublicAPI]
		void EndUpdate();
	}
}