// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider.Configuration;

	// ReSharper disable once InconsistentNaming
	public static class IServiceDescriptorCollectionExtensions
	{
		/// <summary>
		/// Adds the specified <paramref name="descriptor" /> to the <paramref name="services" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection Add(this IServiceDescriptorCollection services, IServiceDescriptor descriptor)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptor, nameof(descriptor));

			services.Add(descriptor);
			return services;
		}

		/// <summary>
		/// Adds a sequence of <see cref="IServiceDescriptor" /> to the <paramref name="services" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection Add(this IServiceDescriptorCollection services, IEnumerable<IServiceDescriptor> descriptors)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptors, nameof(descriptors));

			foreach (IServiceDescriptor descriptor in descriptors)
			{
				services.Add(descriptor);
			}
			return services;
		}

		/// <summary>
		/// Adds the specified <paramref name="descriptor" /> to the <paramref name="services" /> if the
		/// service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAdd(this IServiceDescriptorCollection services, IServiceDescriptor descriptor)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptor, nameof(descriptor));

			if (services.All(d => d.ServiceType != descriptor.ServiceType))
			{
				services.Add(descriptor);
			}
			return services;
		}

		/// <summary>
		/// Adds the specified <paramref name="descriptors" /> to the <paramref name="services" /> if the
		/// service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAdd(this IServiceDescriptorCollection services, IEnumerable<IServiceDescriptor> descriptors)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptors, nameof(descriptors));

			foreach (IServiceDescriptor descriptor in descriptors)
			{
				TryAdd(services, descriptor);
			}
			return services;
		}

		/// <summary>
		/// Adds a transient service of the type specified in <paramref name="serviceType"/> with an
		/// implementation of the type specified in <paramref name="implementationType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient(this IServiceDescriptorCollection services, Type serviceType, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			return Add(services, ServiceDescriptor.Transient(serviceType, implementationType));
		}

		/// <summary>
		/// Adds a transient service of the type specified in <paramref name="serviceType"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient(this IServiceDescriptorCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Add(services, ServiceDescriptor.Transient(serviceType, implementationFactory));
		}

		/// <summary>
		/// Adds a transient service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddTransient(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds a transient service of the type specified in <paramref name="serviceType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient(this IServiceDescriptorCollection services, Type serviceType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			return services.AddTransient(serviceType, serviceType);
		}

		/// <summary>
		/// Adds a transient service of the type specified in <typeparamref name="TService"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddTransient(typeof(TService));
		}

		/// <summary>
		/// Adds a transient service of the type specified in <typeparamref name="TService"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddTransient(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds a transient service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation" /> using the
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddTransient<TService, TImplementation>(this IServiceDescriptorCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddTransient(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient(this IServiceDescriptorCollection services, Type service)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));

			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, service);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// with the <paramref name="implementationType" /> implementation
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient(this IServiceDescriptorCollection services, Type service, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationType);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient(this IServiceDescriptorCollection services, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			ServiceDescriptor descriptor = ServiceDescriptor.Transient(service, implementationFactory);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddTransient(typeof(TService), typeof(TService));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddTransient(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Transient" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddTransient<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			return services.TryAdd(ServiceDescriptor.Transient(implementationFactory));
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <paramref name="serviceType"/> with an
		/// implementation of the type specified in <paramref name="implementationType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped(this IServiceDescriptorCollection services, Type serviceType, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			return Add(services, ServiceDescriptor.Scoped(serviceType, implementationType));
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <paramref name="serviceType"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped(this IServiceDescriptorCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Add(services, ServiceDescriptor.Scoped(serviceType, implementationFactory));
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddScoped(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <paramref name="serviceType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped(this IServiceDescriptorCollection services, Type serviceType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			return services.AddScoped(serviceType, serviceType);
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <typeparamref name="TService"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddScoped(typeof(TService));
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <typeparamref name="TService"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddScoped(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation" /> using the
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddScoped<TService, TImplementation>(this IServiceDescriptorCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddScoped(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped(this IServiceDescriptorCollection services, Type service)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));

			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, service);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// with the <paramref name="implementationType" /> implementation
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped(this IServiceDescriptorCollection services, Type service, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationType);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped(this IServiceDescriptorCollection services, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			ServiceDescriptor descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddScoped(typeof(TService), typeof(TService));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddScoped(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Scoped" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddScoped<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			return services.TryAdd(ServiceDescriptor.Scoped(implementationFactory));
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an
		/// implementation of the type specified in <paramref name="implementationType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton(this IServiceDescriptorCollection services, Type serviceType, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			return Add(services, ServiceDescriptor.Singleton(serviceType, implementationType));
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <paramref name="serviceType"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton(this IServiceDescriptorCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Add(services, ServiceDescriptor.Singleton(serviceType, implementationFactory));
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddSingleton(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <paramref name="serviceType"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton(this IServiceDescriptorCollection services, Type serviceType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			return services.AddSingleton(serviceType, serviceType);
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <typeparamref name="TService"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.AddSingleton(typeof(TService));
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <typeparamref name="TService"/> with a
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddSingleton(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
		/// implementation type specified in <typeparamref name="TImplementation" /> using the
		/// factory specified in <paramref name="implementationFactory"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton<TService, TImplementation>(this IServiceDescriptorCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return services.AddSingleton(typeof(TService), implementationFactory);
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an
		/// instance specified in <paramref name="implementationInstance"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton(this IServiceDescriptorCollection services, Type serviceType, object implementationInstance)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			if (implementationInstance == null)
			{
				throw new ArgumentNullException(nameof(implementationInstance));
			}

			var serviceDescriptor = ServiceDescriptor.Singleton(serviceType, implementationInstance);
			services.Add(serviceDescriptor);
			return services;
		}

		/// <summary>
		/// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
		/// instance specified in <paramref name="implementationInstance"/> to the
		/// specified <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection AddSingleton<TService>(this IServiceDescriptorCollection services, TService implementationInstance)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));
			if (implementationInstance == null)
			{
				throw new ArgumentNullException(nameof(implementationInstance));
			}

			return services.AddSingleton(typeof(TService), implementationInstance);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton(this IServiceDescriptorCollection services, Type service)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));

			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, service);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// with the <paramref name="implementationType" /> implementation
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton(this IServiceDescriptorCollection services, Type service, Type implementationType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationType);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <paramref name="service" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton(this IServiceDescriptorCollection services, Type service, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton<TService>(this IServiceDescriptorCollection services)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddSingleton(typeof(TService), typeof(TService));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// implementation type specified in <typeparamref name="TImplementation" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton<TService, TImplementation>(this IServiceDescriptorCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(services, nameof(services));

			return services.TryAddSingleton(typeof(TService), typeof(TImplementation));
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// with an instance specified in <paramref name="instance" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton<TService>(this IServiceDescriptorCollection services, TService instance)
			where TService : class
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(instance, nameof(instance));

			ServiceDescriptor descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
			return TryAdd(services, descriptor);
		}

		/// <summary>
		/// Adds the specified <typeparamref name="TService" /> as a <see cref="ServiceLifetime.Singleton" /> service
		/// using the factory specified in <paramref name="implementationFactory" />
		/// to the <paramref name="services" /> if the service type hasn't already been registered.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddSingleton<TService>(this IServiceDescriptorCollection services, Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			return services.TryAdd(ServiceDescriptor.Singleton(implementationFactory));
		}

		/// <summary>
		/// Adds a <see cref="IServiceDescriptor" /> if an existing descriptor with the same
		/// <see cref="IServiceDescriptor.ServiceType" /> and an implementation that does not already exist
		/// in <paramref name="services" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddEnumerable(this IServiceDescriptorCollection services, IServiceDescriptor descriptor)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptor, nameof(descriptor));

			Type implementationType = descriptor.ImplementationType;
			if (implementationType == typeof(object) || implementationType == descriptor.ServiceType)
			{
				throw new ArgumentException(string.Format(Resources.TryAddIndistinguishableTypeToEnumerable, implementationType, descriptor.ServiceType), nameof(descriptor));
			}
			if (!services.Any(d =>
			{
				if (d.ServiceType == descriptor.ServiceType)
				{
					return d.ImplementationType == implementationType;
				}
				return false;
			}))
			{
				services.Add(descriptor);
			}
			return services;
		}

		/// <summary>
		/// Adds the specified <see cref="IServiceDescriptor" />s if an existing descriptor with the same
		/// <see cref="IServiceDescriptor.ServiceType" /> and an implementation that does not already exist
		/// in <paramref name="services" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection TryAddEnumerable(this IServiceDescriptorCollection services, IEnumerable<IServiceDescriptor> descriptors)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptors, nameof(descriptors));

			foreach (IServiceDescriptor descriptor in descriptors)
			{
				services.TryAddEnumerable(descriptor);
			}
			return services;
		}

		/// <summary>
		/// Removes the first service in <see cref="IServiceDescriptorCollection" /> with the same service type
		/// as <paramref name="descriptor" /> and adds <paramef name="descriptor" /> to the collection.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection Replace(this IServiceDescriptorCollection services, IServiceDescriptor descriptor)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(descriptor, nameof(descriptor));

			IServiceDescriptor serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == descriptor.ServiceType);
			if (serviceDescriptor != null)
			{
				services.Remove(serviceDescriptor);
			}
			services.Add(descriptor);
			return services;
		}

		/// <summary>
		/// Removes all services of type <typeparamef name="T" /> in <see cref="IServiceDescriptorCollection" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection RemoveAll<T>(this IServiceDescriptorCollection services)
		{
			return services.RemoveAll(typeof(T));
		}

		/// <summary>
		/// Removes all services of type <paramef name="serviceType" /> in <see cref="IServiceDescriptorCollection" />.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptorCollection RemoveAll(this IServiceDescriptorCollection services, Type serviceType)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			for (int idx = services.Count - 1; idx >= 0; --idx)
			{
				if (services[idx].ServiceType == serviceType)
				{
					services.RemoveAt(idx);
				}
			}
			return services;
		}
	}
}