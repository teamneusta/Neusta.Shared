namespace Neusta.Shared.ObjectProvider.Stashbox.Adapter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using global::Stashbox;
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Base;
	using Neusta.Shared.ObjectProvider.Stashbox.Adapter.Helper;

	[UsedImplicitly]
	internal sealed class StashboxContainerAdapter : BaseUpdateableContainerAdapter<StashboxContainerAdapter>
	{
		private readonly StashboxContainer container;
		private readonly IObjectProvider objectProvider;
		private readonly HashSet<Type> unregisteredServiceTypes = new HashSet<Type>();

		/// <summary>
		/// Initializes a new instance of the <see cref="StashboxContainerAdapter" /> class.
		/// </summary>
		public StashboxContainerAdapter(IContainerConfiguration configuration, StashboxContainer container, IObjectProvider objectProvider)
			: base(configuration)
		{
			this.container = container;
			this.objectProvider = objectProvider;
		}

		/// <summary>
		/// Gets the container.
		/// </summary>
		[PublicAPI]
		public StashboxContainer Container
		{
			[DebuggerStepThrough]
			get { return this.container; }
		}

		#region Overrides of BaseContainerAdapter

		/// <summary>
		/// Gets the <see cref="IObjectProvider"/>.
		/// </summary>
		public override IObjectProvider ObjectProvider
		{
			[DebuggerStepThrough]
			get { return this.objectProvider; }
		}

		#endregion

		#region Overrides of BaseUpdateableContainerAdapter

		protected override void InternalRegisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
			base.InternalRegisterServiceDescriptor(serviceDescriptor);

			// Register the services with the container
			Type serviceType = serviceDescriptor.ServiceType;
			bool replaceExisting = this.unregisteredServiceTypes.Contains(serviceType);
			if (replaceExisting)
			{
				this.unregisteredServiceTypes.Remove(serviceType);
			}
			RegistrationHelper.RegisterService(this.container, serviceDescriptor, this.objectProvider, replaceExisting);
		}

		protected override void InternalUnregisterServiceDescriptor(IServiceDescriptor serviceDescriptor)
		{
			base.InternalUnregisterServiceDescriptor(serviceDescriptor);

			// Unregister the service from the container
			Type serviceType = serviceDescriptor.ServiceType;
			RegistrationHelper.UnregisterService(this.container, serviceDescriptor);
			this.unregisteredServiceTypes.Add(serviceType);
		}

		#endregion
	}
}