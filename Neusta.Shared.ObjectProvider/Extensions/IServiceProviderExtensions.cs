// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IServiceProviderExtensions
	{
		/// <summary>
		/// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
		/// </summary>
		[PublicAPI]
		public static T GetService<T>(this IServiceProvider provider)
		{
			Guard.ArgumentNotNull(provider, nameof(provider));

			return (T)provider.GetService(typeof(T));
		}

		/// <summary>
		/// Get service of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
		/// </summary>
		[PublicAPI]
		public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
		{
			Guard.ArgumentNotNull(provider, nameof(provider));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			var service = provider.GetService(serviceType);
			if (service == null)
			{
				throw new InvalidOperationException(string.Format(Resources.NoServiceRegistered, serviceType));
			}

			return service;
		}

		/// <summary>
		/// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
		/// </summary>
		[PublicAPI]
		public static T GetRequiredService<T>(this IServiceProvider provider)
		{
			Guard.ArgumentNotNull(provider, nameof(provider));

			return (T)provider.GetRequiredService(typeof(T));
		}

		/// <summary>
		/// Get an enumeration of services of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
		/// </summary>
		[PublicAPI]
		public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
		{
			Guard.ArgumentNotNull(provider, nameof(provider));

			return provider.GetRequiredService<IEnumerable<T>>();
		}

		/// <summary>
		/// Get an enumeration of services of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
		/// </summary>
		[PublicAPI]
		public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType)
		{
			Guard.ArgumentNotNull(provider, nameof(provider));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
			return (IEnumerable<object>)provider.GetRequiredService(genericEnumerable);
		}
	}
}