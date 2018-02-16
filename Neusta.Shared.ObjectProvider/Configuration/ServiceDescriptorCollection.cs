namespace Neusta.Shared.ObjectProvider.Configuration
{
	using System.Collections.Generic;

	public class ServiceDescriptorCollection : List<IServiceDescriptor>, IServiceDescriptorCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptorCollection"/> class.
		/// </summary>
		internal ServiceDescriptorCollection()
		{
		}
	}
}