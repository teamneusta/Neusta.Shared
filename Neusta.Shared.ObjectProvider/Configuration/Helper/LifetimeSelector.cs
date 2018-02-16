namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;
	using Neusta.Shared.ObjectProvider.Internal;

	internal sealed class LifetimeSelector : ILifetimeSelector, ISelector
	{
		private readonly IServiceTypeSelector inner;
		private readonly IEnumerable<TypeMap> typeMaps;
		private readonly IEnumerable<TypeFactoryMap> typeFactoryMaps;
		private ServiceLifetime? serviceLifetime;

		/// <summary>
		/// Initializes a new instance of the <see cref="LifetimeSelector"/> class.
		/// </summary>
		public LifetimeSelector(IServiceTypeSelector inner, IEnumerable<TypeMap> typeMaps, IEnumerable<TypeFactoryMap> typeFactoryMaps)
		{
			this.inner = inner;
			this.typeMaps = typeMaps;
			this.typeFactoryMaps = typeFactoryMaps;
		}

		#region Explicit Implementation of ILifetimeSelector

		IImplementationTypeSelector ILifetimeSelector.InternalWithLifetime(ServiceLifetime lifetime)
		{
			this.serviceLifetime = lifetime;
			return this;
		}

		#endregion

		#region Explicit Implementation of IAssemblySelector

		Assembly IAssemblySelector.InternalGetApplicationAssembly()
		{
			return this.inner.InternalGetApplicationAssembly();
		}

		IImplementationTypeSelector IAssemblySelector.InternalFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			return this.inner.InternalFromAssemblies(assemblies);
		}

		IImplementationTypeSelector IAssemblySelector.InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos)
		{
			return this.inner.InternalFromAssembliesOf(typeInfos);
		}

		#endregion

		#region Explicit Implementation of IImplementationTypeSelector

		public IServiceTypeSelector InternalAddClasses(Action<IImplementationTypeFilter> action, bool publicOnly)
		{
			return this.inner.InternalAddClasses(action, publicOnly);
		}

		#endregion

		#region Implementation of IServiceTypeSelector

		IEnumerable<Type> IServiceTypeSelector.InternalTypes
		{
			[DebuggerStepThrough]
			get { return this.inner.InternalTypes; }
		}

		ILifetimeSelector IServiceTypeSelector.InternalAddSelector(IEnumerable<TypeMap> types, IEnumerable<TypeFactoryMap> factories)
		{
			return this.inner.InternalAddSelector(types, factories);
		}

		IImplementationTypeSelector IServiceTypeSelector.InternalUsingAttributes()
		{
			return this.inner.InternalUsingAttributes();
		}

		IServiceTypeSelector IServiceTypeSelector.InternalUsingRegistrationStrategy(RegistrationStrategy registrationStrategy)
		{
			return this.inner.InternalUsingRegistrationStrategy(registrationStrategy);
		}

		#endregion

		#region Explicit Implementation of ISelector

		void ISelector.Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy)
		{
			var serviceLifetime = this.serviceLifetime.GetValueOrDefault(ServiceLifetime.Transient);

			foreach (var typeMap in this.typeMaps)
			{
				foreach (var serviceType in typeMap.ServiceTypes)
				{
					var implementationType = typeMap.ImplementationType;
					if (!implementationType.IsAssignableTo(serviceType))
					{
						throw new InvalidOperationException(string.Format(Resources.TypeNotAssignableTo, implementationType.ToFriendlyName(), serviceType.ToFriendlyName()));
					}
					var descriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);
					descriptor.ApplyAttributes(implementationType);
					strategyApplier.Apply(descriptor, strategy);
				}
			}

			foreach (var typeFactoryMap in this.typeFactoryMaps)
			{
				foreach (var serviceType in typeFactoryMap.ServiceTypes)
				{
					var descriptor = ServiceDescriptor.Describe(serviceType, typeFactoryMap.ImplementationFactory, serviceLifetime);
					strategyApplier.Apply(descriptor, strategy);
				}
			}
		}

		#endregion
	}
}