// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider.Configuration
{
	using System;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;

	public static class ServiceDescriptorExtensions
	{
		/// <summary>
		/// Applies the implementation type attributes to the service descriptor.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor ApplyAttributes(this ServiceDescriptor serviceDescriptor)
		{
			return serviceDescriptor.ApplyAttributes(serviceDescriptor.ImplementationType);
		}

		/// <summary>
		/// Applies the implementation type attributes to the service descriptor.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor ApplyAttributes(this ServiceDescriptor serviceDescriptor, Type type)
		{
			var typeInfo = type?.GetTypeInfo();
			if (typeInfo != null)
			{
				// Check for disposal tracking
				if (typeInfo.GetCustomAttribute<DisableDisposalTrackingAttribute>() != null)
				{
					serviceDescriptor.DisableDisposalTracking = true;
				}

				// Check for member injection mode
				var memberInjectionAttribute = typeInfo.GetCustomAttributes<MemberInjectionAttribute>().FirstOrDefault();
				if (memberInjectionAttribute != null)
				{
					serviceDescriptor.MemberInjectionMode = memberInjectionAttribute.MemberInjectionMode;
				}

				// Check for lifetime
				var serviceLifetimeAttribute = typeInfo.GetCustomAttributes<ServiceLifetimeAttribute>().FirstOrDefault();
				if (serviceLifetimeAttribute != null)
				{
					serviceDescriptor.ServiceLifetime = serviceLifetimeAttribute.ServiceLifetime;
				}
			}
			return serviceDescriptor;
		}
	}
}