namespace Neusta.Shared.ObjectProvider.Configuration
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	[DebuggerDisplay("Lifetime = {ServiceLifetime}, ServiceType = {ServiceType.Name}, ImplementationType = {ImplementationType.Name}, Source = {ImplementationSource}")]
	public class ServiceDescriptor : IServiceDescriptor
	{
		private readonly Type serviceType;
		private readonly ImplementationSource implementationSource;
		private readonly Type implementationType;
		private readonly object implementationInstance;
		private readonly Func<object> implementationFactory;
		private readonly Func<IServiceProvider, object> implementationFactoryWithProvider;
		private ServiceLifetime lifetime;
		private MemberInjectionMode memberInjectionMode;
		private bool disableDisposalTracking;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
		/// </summary>
		public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));
			if (!implementationType.IsNonAbstractClass(false))
			{
				throw new InvalidOperationException(string.Format("Abstract class {0} is not allowed as implementation type.", implementationType.ToFriendlyName()));
			}

			this.serviceType = serviceType;
			this.implementationType = implementationType;
			this.lifetime = lifetime;
			this.implementationSource = ImplementationSource.Type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
		/// </summary>
		public ServiceDescriptor(Type serviceType, Func<object> implementationFactory, ServiceLifetime lifetime)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			this.serviceType = serviceType;
			this.implementationFactory = implementationFactory;
			this.implementationFactoryWithProvider = this.InvokeImplementationFactory;
			this.lifetime = lifetime;

			var typeArguments = this.implementationFactory.GetType().GenericTypeArguments;
			if (typeArguments.Length == 1)
			{
				this.implementationType = typeArguments[0];
				this.implementationSource = ImplementationSource.Factory;
			}
			else
			{
				this.implementationSource = ImplementationSource.FactoryInvalid;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
		/// </summary>
		public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactoryWithProvider, ServiceLifetime lifetime)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactoryWithProvider, nameof(implementationFactoryWithProvider));

			this.serviceType = serviceType;
			this.implementationFactoryWithProvider = implementationFactoryWithProvider;
			this.lifetime = lifetime;

			var typeArguments = this.implementationFactoryWithProvider.GetType().GenericTypeArguments;
			if (typeArguments.Length == 2)
			{
				this.implementationType = typeArguments[1];
				this.implementationSource = ImplementationSource.FactoryWithProvider;
			}
			else
			{
				this.implementationSource = ImplementationSource.FactoryInvalid;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
		/// </summary>
		public ServiceDescriptor(Type serviceType, object implementationInstance)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			if ((implementationInstance != null) && !implementationInstance.GetType().IsAssignableTo(serviceType))
			{
				throw new InvalidOperationException(string.Format("Instance of type {0} is not assignable to service type {1}.", implementationInstance.GetType().ToFriendlyName(), serviceType.ToFriendlyName()));
			}

			this.serviceType = serviceType;
			this.implementationInstance = implementationInstance;
			this.lifetime = ServiceLifetime.Singleton;
			this.implementationSource = ImplementationSource.Instance;

			if (this.implementationInstance != null)
			{
				this.implementationType = this.implementationInstance.GetType();
			}
			else
			{
				this.implementationType = typeof(void);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
		/// </summary>
		protected ServiceDescriptor(Type serviceType, Type implementationType, object implementationInstance, Func<IServiceProvider, object> implementationFactoryWithProvider, ServiceLifetime lifetime)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));

			this.serviceType = serviceType;
			this.implementationType = implementationType;
			this.implementationInstance = implementationInstance;
			this.implementationFactoryWithProvider = implementationFactoryWithProvider;
			this.lifetime = lifetime;

			if (this.implementationType != null)
			{
				this.implementationSource = ImplementationSource.Type;
			}
			else if (this.implementationInstance != null)
			{
				this.implementationType = this.implementationInstance.GetType();
				this.implementationSource = ImplementationSource.Instance;
			}
			else if (this.implementationFactoryWithProvider != null)
			{
				var typeArguments = this.implementationFactoryWithProvider.GetType().GenericTypeArguments;
				if (typeArguments.Length == 2)
				{
					this.implementationType = typeArguments[1];
					this.implementationSource = ImplementationSource.FactoryWithProvider;
				}
				else
				{
					this.implementationSource = ImplementationSource.FactoryInvalid;
				}
			}
			else if (this.lifetime == ServiceLifetime.Singleton)
			{
				this.implementationType = typeof(void);
				this.implementationSource = ImplementationSource.Instance;
			}
			else
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		#region Implementation of IServiceDescriptor

		/// <summary>
		/// Gets the service type.
		/// </summary>
		public Type ServiceType
		{
			[DebuggerStepThrough]
			get { return this.serviceType; }
		}

		/// <summary>
		/// Gets the resolved implementation source.
		/// </summary>
		public ImplementationSource ImplementationSource
		{
			[DebuggerStepThrough]
			get { return this.implementationSource; }
		}

		/// <summary>
		/// Gets or sets the implementation type.
		/// </summary>
		public Type ImplementationType
		{
			[DebuggerStepThrough]
			get { return this.implementationType; }
		}

		/// <summary>
		/// Gets or sets the implementation instance.
		/// </summary>>
		public object ImplementationInstance
		{
			[DebuggerStepThrough]
			get { return this.implementationInstance; }
		}

		/// <summary>
		/// Gets or sets the implementation factory.
		/// </summary>
		public Func<object> ImplementationFactory
		{
			[DebuggerStepThrough]
			get { return this.implementationFactory; }
		}

		/// <summary>
		/// Gets or sets the implementation factory.
		/// </summary>
		public Func<IServiceProvider, object> ImplementationFactoryWithProvider
		{
			[DebuggerStepThrough]
			get { return this.implementationFactoryWithProvider; }
		}

		/// <summary>
		/// Gets or sets the lifetime.
		/// </summary>
		public ServiceLifetime ServiceLifetime
		{
			[DebuggerStepThrough]
			get { return this.lifetime; }
			[DebuggerStepThrough]
			set { this.lifetime = value; }
		}

		/// <summary>
		/// Gets or sets the member injection mode.
		/// </summary>
		public MemberInjectionMode MemberInjectionMode
		{
			[DebuggerStepThrough]
			get { return this.memberInjectionMode; }
			[DebuggerStepThrough]
			set { this.memberInjectionMode = value; }
		}

		/// <summary>
		/// Gets a value indicating whether disposal tracking is disabled.
		/// </summary>
		public bool DisableDisposalTracking
		{
			[DebuggerStepThrough]
			get { return this.disableDisposalTracking; }
			[DebuggerStepThrough]
			set { this.disableDisposalTracking = value; }
		}

		#endregion

		#region Private Methods

		[DebuggerNonUserCode]
		private object InvokeImplementationFactory(IServiceProvider provider) => this.implementationFactory();

		#endregion

		#region Static Factory Methods

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// and the <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static IServiceDescriptor Transient<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			return Describe<TService, TImplementation>(ServiceLifetime.Transient);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="service"/> and <paramref name="implementationType"/>
		/// and the <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Transient(Type service, Type implementationType)
		{
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			return Describe(service, implementationType, ServiceLifetime.Transient);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Transient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="service"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(service, implementationFactory, ServiceLifetime.Transient);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Scoped<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			return Describe<TService, TImplementation>(ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="service"/> and <paramref name="implementationType"/>
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Scoped(Type service, Type implementationType)
		{
			return Describe(service, implementationType, ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Scoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="service"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(service, implementationFactory, ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// and the <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <typeparam name="TImplementation">The type of the implementation.</typeparam>
		/// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
		[PublicAPI]
		public static ServiceDescriptor Singleton<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			return Describe<TService, TImplementation>(ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="service"/> and <paramref name="implementationType"/>
		/// and the <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton(Type service, Type implementationType)
		{
			Guard.ArgumentNotNull(service, nameof(service));
			Guard.ArgumentNotNull(implementationType, nameof(implementationType));

			return Describe(service, implementationType, ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory)
			where TService : class
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="serviceType"/>, <paramref name="implementationFactory"/>,
		/// and the <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(serviceType, implementationFactory, ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <paramref name="implementationInstance"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton<TService>(TService implementationInstance)
			where TService : class
		{
			Guard.ArgumentNotNull(implementationInstance, nameof(implementationInstance));

			return Singleton(typeof(TService), implementationInstance);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="serviceType"/>, <paramref name="implementationInstance"/>,
		/// and the <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Singleton(Type serviceType, object implementationInstance)
		{
			Guard.ArgumentNotNull(serviceType, nameof(serviceType));
			Guard.ArgumentNotNull(implementationInstance, nameof(implementationInstance));

			return new ServiceDescriptor(serviceType, implementationInstance);
		}

		[PublicAPI]
		public static ServiceDescriptor Describe<TService, TImplementation>(ServiceLifetime lifetime)
			where TService : class
			where TImplementation : class, TService
		{
			return Describe(typeof(TService), typeof(TImplementation), lifetime: lifetime);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="serviceType"/>, <paramref name="implementationType"/>,
		/// and <paramref name="lifetime"/>.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Describe(Type serviceType, Type implementationType, ServiceLifetime lifetime)
		{
			return new ServiceDescriptor(serviceType, implementationType, lifetime);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
		/// <paramref name="implementationFactory"/> and <paramref name="lifetime"/>.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Describe<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime)
			where TService : class
			where TImplementation : class, TService
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, lifetime);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <typeparamref name="TService"/>, <paramref name="implementationFactory"/> and <paramref name="lifetime"/>.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Describe<TService>(Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
			where TService : class
		{
			Guard.ArgumentNotNull(implementationFactory, nameof(implementationFactory));

			return Describe(typeof(TService), implementationFactory, lifetime);
		}

		/// <summary>
		/// Creates an instance of <see cref="ServiceDescriptor"/> with the specified
		/// <paramref name="serviceType"/>, <paramref name="implementationFactory"/>
		/// and <paramref name="lifetime"/>.
		/// </summary>
		[PublicAPI]
		public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
		{
			return new ServiceDescriptor(serviceType, implementationFactory, lifetime);
		}

		#endregion
	}
}