namespace Neusta.Shared.AspNetCore.Extensions.ContainerService
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	[UsedImplicitly]
	internal sealed class ServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
	{
		private IContainerAdapter containerAdapter;
		private IServiceProvider serviceProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceProviderFactory"/> class.
		/// </summary>
		public ServiceProviderFactory(IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			this.containerAdapter = containerAdapter;
		}

		/// <summary>
		/// Gets the <see cref="IContainerAdapter" />.
		/// </summary>
		[PublicAPI]
		public IContainerAdapter ContainerAdapter
		{
			[DebuggerStepThrough]
			get { return this.containerAdapter; }
		}

		#region Implementation of IServiceProviderFactory<IServiceCollection>

		/// <summary>
		/// Creates a container builder from an <see cref="IServiceCollection" />.
		/// </summary>
		public IServiceCollection CreateBuilder(IServiceCollection services)
		{
			// Add a service registrations for the current service provider factory
			services.Replace(ServiceDescriptor.Transient<IServiceProvider>(_ => this.serviceProvider));
			services.Replace(ServiceDescriptor.Transient<IServiceProviderFactory<IServiceCollection>>(_ => this));

			return services;
		}

		/// <summary>
		/// Creates an <see cref="IServiceProvider" /> from the container builder.
		/// </summary>
		public IServiceProvider CreateServiceProvider(IServiceCollection services)
		{
			if (this.containerAdapter is IUpdateableContainerAdapter updateableContainerAdapter)
			{
				// Update service registrations
				updateableContainerAdapter.BeginUpdate();
				try
				{
					ICollection<TranslatedServiceDescriptor> existingServices = updateableContainerAdapter.ServiceDescriptors.OfType<TranslatedServiceDescriptor>().ToList();
					ICollection<ServiceDescriptor> newServices = services.ToList();
					ICollection<Type> typesToReRegister = new HashSet<Type>();
					if (existingServices.Any())
					{
						foreach (var service in existingServices)
						{
							if (!newServices.Contains(service.ServiceDescriptor))
							{
								typesToReRegister.Add(service.ServiceType);
							}
						}
						ICollection<TranslatedServiceDescriptor> outdatedServices = existingServices.Where(match => typesToReRegister.Contains(match.ServiceType)).ToList();
						foreach (var service in outdatedServices)
						{
							updateableContainerAdapter.UnregisterServiceDescriptor(service);
						}
					}
					if (newServices.Any())
					{
						foreach (var service in newServices)
						{
							if (existingServices.All(match => match.ServiceDescriptor != service) || typesToReRegister.Contains(service.ServiceType))
							{
								var translatedService = new TranslatedServiceDescriptor(service);
								updateableContainerAdapter.RegisterServiceDescriptor(translatedService);
							}
						}
					}
				}
				finally
				{
					updateableContainerAdapter.EndUpdate();
				}

				// Create the service provider only if necessary
				if (this.serviceProvider == null)
				{
					this.serviceProvider = new ServiceProvider(this.containerAdapter);
				}
			}
			else
			{
				var configuration = this.containerAdapter.Configuration;

				// Patch configuration
				ICollection<TranslatedServiceDescriptor> existingServices = configuration.ServiceDescriptors.OfType<TranslatedServiceDescriptor>().ToList();
				ICollection<ServiceDescriptor> newServices = services.ToList();
				ICollection<Type> typesToReRegister = new HashSet<Type>();
				if (existingServices.Any())
				{
					foreach (var service in existingServices)
					{
						if (!newServices.Contains(service.ServiceDescriptor))
						{
							typesToReRegister.Add(service.ServiceType);
						}
					}
					ICollection<TranslatedServiceDescriptor> outdatedServices = existingServices.Where(match => typesToReRegister.Contains(match.ServiceType)).ToList();
					foreach (var service in outdatedServices)
					{
						configuration.ServiceDescriptors.Remove(service);
					}
				}
				if (newServices.Any())
				{
					foreach (var service in newServices)
					{
						if (existingServices.All(match => match.ServiceDescriptor != service) || typesToReRegister.Contains(service.ServiceType))
						{
							var translatedService = new TranslatedServiceDescriptor(service);
							configuration.ServiceDescriptors.Add(translatedService);
						}
					}
				}

				// Need to create a new container adapter each time
				this.containerAdapter = ContainerBuilder.From(configuration).Build();
				this.serviceProvider = new ServiceProvider(this.containerAdapter);
			}

			return this.serviceProvider;
		}

		#endregion
	}
}