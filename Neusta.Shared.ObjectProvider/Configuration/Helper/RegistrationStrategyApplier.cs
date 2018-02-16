namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using Neusta.Shared.ObjectProvider.Internal;

	internal sealed class RegistrationStrategyApplier : IRegistrationStrategyApplier
	{
		private readonly IServiceDescriptorCollection serviceCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationStrategyApplier"/> class.
		/// </summary>
		public RegistrationStrategyApplier(IServiceDescriptorCollection serviceCollection)
		{
			this.serviceCollection = serviceCollection;
		}

		#region Implementation of IRegistrationStrategyApplier

		/// <summary>
		/// Applies the strategy for the specified service descriptors.
		/// </summary>
		public void Apply(IServiceDescriptor descriptor, RegistrationStrategy strategy)
		{
			switch (strategy)
			{
				case RegistrationStrategy.Append:
					this.serviceCollection.Add(descriptor);
					break;
				case RegistrationStrategy.Skip:
					this.serviceCollection.TryAdd(descriptor);
					break;
				case RegistrationStrategy.ReplaceByServiceType:
					this.RemoveAllByServiceType(descriptor.ServiceType);
					this.serviceCollection.Add(descriptor);
					break;
				case RegistrationStrategy.ReplaceByImplementationType:
					this.RemoveAllByImplementationType(descriptor.ServiceType);
					this.serviceCollection.Add(descriptor);
					break;
				case RegistrationStrategy.ReplaceByServiceAndImplementationType:
					this.RemoveAllByServiceType(descriptor.ServiceType);
					this.RemoveAllByImplementationType(descriptor.ServiceType);
					this.serviceCollection.Add(descriptor);
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

		#endregion
	}
}