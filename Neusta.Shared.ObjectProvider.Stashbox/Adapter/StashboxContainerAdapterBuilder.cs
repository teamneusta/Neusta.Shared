namespace Neusta.Shared.ObjectProvider.Stashbox.Adapter
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using CommonServiceLocator;
	using global::Stashbox;
	using global::Stashbox.Registration;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;
	using Neusta.Shared.ObjectProvider.Base;
	using Neusta.Shared.ObjectProvider.Stashbox.Adapter.Helper;
	using Neusta.Shared.ObjectProvider.Stashbox.Service;

	[UsedImplicitly]
	internal sealed class StashboxContainerAdapterBuilder : BaseContainerAdapterBuilder
	{
		private static readonly ILogger logger = LogManager.GetLogger<StashboxContainerAdapterBuilder>();
		private static readonly TypeInfo SingletonTypeInfo = typeof(ISingleton).GetTypeInfo();

		private Action<IContainerConfigurator> configure;

		/// <summary>
		/// Initializes a new instance of the <see cref="StashboxContainerAdapterBuilder"/> class.
		/// </summary>>
		public StashboxContainerAdapterBuilder()
		{
		}

		/// <summary>
		/// Gets or sets the configuration action.
		/// </summary>
		public Action<IContainerConfigurator> Configure
		{
			[DebuggerStepThrough]
			get { return this.configure; }
			[DebuggerStepThrough]
			set { this.configure = value; }
		}

		#region Implementation of IContainerServiceBuilder

		/// <summary>
		/// Builds the container object provider.
		/// </summary>
		public override IContainerAdapter Build(IContainerConfiguration configuration)
		{
			// Build stashbox container and the object provider
			var container = new StashboxContainer();
			var objectProvider = new StashboxObjectProvider(container);

			// Configure the container
			container.Configure(delegate (IContainerConfigurator configurator)
			{
				configurator
					.WithCircularDependencyWithLazy()
					.WithCircularDependencyTracking()
					.WithUniqueRegistrationIdentifiers()
					.WithDisposableTransientTracking();
				if (configuration.AutoResolveUnknownTypes)
				{
					configurator.WithUnknownTypeResolution(context => this.ResolveUnknownType(context, configuration, objectProvider));
				}
				this.Configure?.Invoke(configurator);
			});

			// Register services and replace existing registrations
			int cnt = 0;
			int srv = 0;
			var serviceGroups = configuration.ServiceDescriptors.GroupBy(x => x.ServiceType);
			foreach (var serviceGroup in serviceGroups)
			{
				var serviceDescriptors = serviceGroup.ToArray();
				if (!configuration.AutoResolveUnknownTypes || (serviceDescriptors.Length > 1))
				{
					foreach (IServiceDescriptor serviceDescriptor in serviceDescriptors)
					{
						RegistrationHelper.RegisterService(container, serviceDescriptor, objectProvider, false);
						cnt++;
					}
					srv++;
				}
				else
				{
					var serviceDescriptor = serviceDescriptors[0];
					if (!serviceDescriptor.IsSelfBoundService() || serviceDescriptor.IsSingletonBoundService())
					{
						RegistrationHelper.RegisterService(container, serviceDescriptor, objectProvider, false);
						cnt++;
						srv++;
					}
				}
			}
			logger.Trace("Registered {0} descriptors for {1} services with root container.", cnt, srv);

			// Register the root object provider
			container.RegisterTypes(new Type[]
			{
				typeof(IObjectProviderRoot),
				typeof(IObjectProvider),
				typeof(IServiceLocator),
				typeof(IServiceProvider),
			}, null, context => context
				.ReplaceExisting()
				.WithInstance(objectProvider)
				.WithoutDisposalTracking());

			// Build the root object provider
			return new StashboxContainerAdapter(configuration, container, objectProvider);
		}

		#endregion

		#region Private Methods

		private void ResolveUnknownType(IFluentServiceRegistrator context, IContainerConfiguration configuration, StashboxObjectProvider objectProvider)
		{
			Type serviceType = context.ServiceType;

			// Check for matching service descriptors
			IServiceDescriptor serviceDescriptor = configuration.ServiceDescriptors.FirstOrDefault(match => match.ServiceType == serviceType);
			if ((serviceDescriptor != null) && !serviceDescriptor.IsSingletonBoundService())
			{
				logger.Trace("Auto-Resolving type {0} from service descriptor.", serviceType.ToFriendlyName());
				RegistrationHelper.ConfigureContextFromServiceDescriptor(context, serviceDescriptor, objectProvider);
			}
			else
			{
				// Register type directory
				TypeInfo typeInfo = serviceType.GetTypeInfo();
				bool isSingleton = typeInfo.GetCustomAttribute<SingletonAttribute>(true) != null || SingletonTypeInfo.IsAssignableFrom(typeInfo);
				if (isSingleton)
				{
					logger.Trace("Auto-Resolving singleton type {0}.", serviceType.ToFriendlyName());
					var propertyInfo = SingletonHelper.GetSingletonInstancePropertyInfo(serviceType);
					RegistrationHelper.ConfigureContextForSingleton(context, propertyInfo, objectProvider);
				}
				else if (typeInfo.IsClass)
				{
					logger.Trace("Auto-Resolving type {0} as implementing types.", serviceType.ToFriendlyName());
					RegistrationHelper.ConfigureContextForService(context, typeInfo, objectProvider);
				}
				else
				{
					logger.Trace("Unable to auto-resolve service type {0}.", serviceType.ToFriendlyName());
				}
			}
		}

		#endregion
	}
}