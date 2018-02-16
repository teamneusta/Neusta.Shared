namespace Neusta.Shared.ObjectProvider.NetStandard.Utils
{
	using System;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Neusta.Shared.ObjectProvider.Internal;

	internal sealed class RegistrationStrategyApplier : IRegistrationStrategyApplier
	{
		private readonly IServiceCollection serviceCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationStrategyApplier"/> class.
		/// </summary>
		public RegistrationStrategyApplier(IServiceCollection serviceCollection)
		{
			this.serviceCollection = serviceCollection;
		}

		#region Implementation of IRegistrationStrategyApplier

		/// <summary>
		/// Applies the strategy for the specified service descriptors.
		/// </summary>
		public void Apply(IServiceDescriptor descriptor, RegistrationStrategy strategy)
		{
			var translatedDescriptor = TranslateDescriptor(descriptor);
			switch (strategy)
			{
				case RegistrationStrategy.Append:
					this.serviceCollection.Add(translatedDescriptor);
					break;
				case RegistrationStrategy.Skip:
					this.serviceCollection.TryAdd(translatedDescriptor);
					break;
				case RegistrationStrategy.ReplaceByServiceType:
					this.RemoveAllByServiceType(descriptor.ServiceType);
					this.serviceCollection.Add(translatedDescriptor);
					break;
				case RegistrationStrategy.ReplaceByImplementationType:
					this.RemoveAllByImplementationType(descriptor.ServiceType);
					this.serviceCollection.Add(translatedDescriptor);
					break;
				case RegistrationStrategy.ReplaceByServiceAndImplementationType:
					this.RemoveAllByServiceType(translatedDescriptor.ServiceType);
					this.RemoveAllByImplementationType(translatedDescriptor.ServiceType);
					this.serviceCollection.Add(translatedDescriptor);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
			}
		}

		#endregion

		#region Private Methods

		private void RemoveAllByServiceType(Type serviceType)
		{
			for (var idx = this.serviceCollection.Count - 1; idx >= 0; idx--)
			{
				if (this.serviceCollection[idx].ServiceType == serviceType)
				{
					this.serviceCollection.RemoveAt(idx);
				}
			}
		}

		private void RemoveAllByImplementationType(Type implementationType)
		{
			for (var idx = this.serviceCollection.Count - 1; idx >= 0; idx--)
			{
				if (this.serviceCollection[idx].ImplementationType == implementationType)
				{
					this.serviceCollection.RemoveAt(idx);
				}
			}
		}

		private static ServiceDescriptor TranslateDescriptor(IServiceDescriptor descriptor)
		{
			Type serviceType = descriptor.ServiceType;

			ServiceLifetime translatedLifetime;
			switch (descriptor.ServiceLifetime)
			{
				case global::Neusta.Shared.ObjectProvider.ServiceLifetime.Singleton:
					translatedLifetime = global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton;
					break;
				case global::Neusta.Shared.ObjectProvider.ServiceLifetime.Scoped:
					translatedLifetime = global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped;
					break;
				case global::Neusta.Shared.ObjectProvider.ServiceLifetime.Transient:
					translatedLifetime = global::Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			ServiceDescriptor translatedDescriptor;
			switch (descriptor.ImplementationSource)
			{
				case ImplementationSource.Type:
					translatedDescriptor = ServiceDescriptor.Describe(serviceType, descriptor.ImplementationType, translatedLifetime);
					break;
				case ImplementationSource.Instance:
					translatedDescriptor = ServiceDescriptor.Singleton(serviceType, descriptor.ImplementationInstance);
					break;
				case ImplementationSource.Factory:
					translatedDescriptor = ServiceDescriptor.Describe(serviceType, descriptor.ImplementationFactoryWithProvider, translatedLifetime);
					break;
				case ImplementationSource.FactoryWithProvider:
					translatedDescriptor = ServiceDescriptor.Describe(serviceType, descriptor.ImplementationFactoryWithProvider, translatedLifetime);
					break;
				case ImplementationSource.FactoryInvalid:
					throw new NotSupportedException();
				default:
					throw new ArgumentOutOfRangeException();
			}

			return translatedDescriptor;
		}

		#endregion
	}
}