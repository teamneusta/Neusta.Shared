namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Neusta.Shared.ObjectProvider.Internal;

	internal class AttributeSelector : ISelector
	{
		private readonly IEnumerable<Type> types;

		public AttributeSelector(IEnumerable<Type> types)
		{
			this.types = types;
		}

		#region Explicit Implementation of ISelector

		void ISelector.Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy)
		{
			foreach (var type in this.types)
			{
				var typeInfo = type.GetTypeInfo();

				// Check if the type has multiple attributes with same ServiceType.
				var serviceDescriptorAttributes = typeInfo.GetCustomAttributes<ServiceDescriptorAttribute>().ToArray();
				if (GetDuplicates(serviceDescriptorAttributes).Any())
				{
					throw new InvalidOperationException(string.Format(Resources.MultipleAttributesWithSameServiceType, type.ToFriendlyName()));
				}

				foreach (ServiceDescriptorAttribute attribute in serviceDescriptorAttributes)
				{
					var serviceTypes = attribute.GetServiceTypes(type);
					foreach (var serviceType in serviceTypes)
					{
						var descriptor = ServiceDescriptor.Describe(serviceType, type, attribute.ServiceLifetime);
						descriptor.ApplyAttributes(type);
						strategyApplier.Apply(descriptor, strategy);
					}
				}
			}
		}

		#endregion

		#region Private Methods

		private static IEnumerable<ServiceDescriptorAttribute> GetDuplicates(IEnumerable<ServiceDescriptorAttribute> attributes)
		{
			return attributes.GroupBy(s => s.ServiceType).SelectMany(grp => grp.Skip(1));
		}

		#endregion
	}
}