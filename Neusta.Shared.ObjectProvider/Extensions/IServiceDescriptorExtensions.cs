// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IServiceDescriptorExtensions
	{
		private static readonly TypeInfo SingletonTypeInfo = typeof(ISingleton).GetTypeInfo();

		/// <summary>
		/// Determines whether the service descriptor describes a service that has a singleton implementation.
		/// </summary>
		[PublicAPI]
		public static bool IsSingletonBoundService(this IServiceDescriptor serviceDescriptor)
		{
			TypeInfo typeInfo = serviceDescriptor.ImplementationType?.GetTypeInfo();
			if (typeInfo == null)
			{
				return false;
			}
			return typeInfo.GetCustomAttribute<SingletonAttribute>(true) != null || SingletonTypeInfo.IsAssignableFrom(typeInfo);
		}

		/// <summary>
		/// Determines whether the service descriptor describes a self bound service.
		/// </summary>
		[PublicAPI]
		public static bool IsSelfBoundService(this IServiceDescriptor serviceDescriptor)
		{
			if (serviceDescriptor.ImplementationSource != ImplementationSource.Type)
			{
				return false;
			}
			return serviceDescriptor.ServiceType == serviceDescriptor.ImplementationType;
		}
	}
}