namespace Neusta.Shared.ObjectProvider.Configuration
{
	using System.Diagnostics;
	using System.Reflection;

	public class ContainerConfiguration : IContainerConfiguration
	{
		private readonly Assembly applicationAssembly;
		private bool autoResolveUnknownTypes;
		private readonly IServiceDescriptorCollection serviceDescriptors;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerConfiguration"/> class.
		/// </summary>
		protected internal ContainerConfiguration()
		{
			this.serviceDescriptors = new ServiceDescriptorCollection();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerConfiguration"/> class.
		/// </summary>
		protected internal ContainerConfiguration(Assembly applicationAssembly)
			: this()
		{
			this.applicationAssembly = applicationAssembly;
		}

		#region Implementation of IContainerBuilderData

		/// <summary>
		/// Gets the main <see cref="Assembly" />.
		/// </summary>
		public Assembly ApplicationAssembly
		{
			[DebuggerStepThrough]
			get { return this.applicationAssembly; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether unknown types should be resolved automatically.
		/// </summary>
		public bool AutoResolveUnknownTypes
		{
			[DebuggerStepThrough]
			get { return this.autoResolveUnknownTypes; }
			[DebuggerStepThrough]
			set { this.autoResolveUnknownTypes = value; }
		}

		/// <summary>
		/// Gets the service descriptors.
		/// </summary>
		public IServiceDescriptorCollection ServiceDescriptors
		{
			[DebuggerStepThrough]
			get { return this.serviceDescriptors; }
		}

		#endregion
	}
}