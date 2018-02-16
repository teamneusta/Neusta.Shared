namespace Neusta.Shared.AspNetCore.Extensions.ContainerService
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class TranslatedServiceDescriptor : global::Neusta.Shared.ObjectProvider.Configuration.ServiceDescriptor
	{
		private readonly global::Microsoft.Extensions.DependencyInjection.ServiceDescriptor serviceDescriptor;

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedServiceDescriptor"/> class.
		/// </summary>
		public TranslatedServiceDescriptor(global::Microsoft.Extensions.DependencyInjection.ServiceDescriptor serviceDescriptor)
			: base(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType, serviceDescriptor.ImplementationInstance, serviceDescriptor.ImplementationFactory, TranslateServiceLifetime(serviceDescriptor))
		{
			this.serviceDescriptor = serviceDescriptor;
		}

		/// <summary>
		/// Gets the translated service descriptor.
		/// </summary>
		[PublicAPI]
		public global::Microsoft.Extensions.DependencyInjection.ServiceDescriptor ServiceDescriptor
		{
			[DebuggerStepThrough]
			get { return this.serviceDescriptor; }
		}

		#region Private Methods

		private static global::Neusta.Shared.ObjectProvider.ServiceLifetime TranslateServiceLifetime(ServiceDescriptor serviceDescriptor)
		{
			switch (serviceDescriptor.Lifetime)
			{
				case global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
					return global::Neusta.Shared.ObjectProvider.ServiceLifetime.Singleton;
				case global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
					return global::Neusta.Shared.ObjectProvider.ServiceLifetime.Scoped;
				case global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient:
					return global::Neusta.Shared.ObjectProvider.ServiceLifetime.Transient;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion
	}
}