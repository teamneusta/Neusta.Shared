namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider.Internal;

	internal class ServiceTypeSelector : IServiceTypeSelector, ISelector
	{
		private readonly IImplementationTypeSelector inner;
		private readonly IEnumerable<Type> types;
		private readonly List<ISelector> selectors;
		private Nullable<RegistrationStrategy> registrationStrategy;

		public ServiceTypeSelector(IImplementationTypeSelector inner, IEnumerable<Type> types)
		{
			this.inner = inner;
			this.types = types;
			this.selectors = new List<ISelector>();
		}

		#region Explicit Implementation of IServiceTypeSelector

		IEnumerable<Type> IServiceTypeSelector.InternalTypes
		{
			[DebuggerStepThrough]
			get { return this.types; }
		}

		ILifetimeSelector IServiceTypeSelector.InternalAddSelector(IEnumerable<TypeMap> types, IEnumerable<TypeFactoryMap> factories)
		{
			var selector = new LifetimeSelector(this, types, factories);
			this.selectors.Add(selector);
			return selector;
		}

		IImplementationTypeSelector IServiceTypeSelector.InternalUsingAttributes()
		{
			var selector = new AttributeSelector(this.types);
			this.selectors.Add(selector);
			return this;
		}

		IServiceTypeSelector IServiceTypeSelector.InternalUsingRegistrationStrategy(RegistrationStrategy registrationStrategy)
		{
			Guard.ArgumentNotNull(registrationStrategy, nameof(registrationStrategy));

			this.registrationStrategy = registrationStrategy;
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

		IServiceTypeSelector IImplementationTypeSelector.InternalAddClasses(Action<IImplementationTypeFilter> action, bool publicOnly)
		{
			return this.inner.InternalAddClasses(action, publicOnly);
		}

		#endregion

		#region Explicit Implementation of ISelector

		void ISelector.Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy)
		{
			if (this.selectors.Count == 0)
			{
				this.AsSelfWithInterfaces();
			}
			strategy = this.registrationStrategy ?? strategy;
			foreach (var selector in this.selectors)
			{
				selector.Populate(strategyApplier, strategy);
			}
		}

		#endregion
	}
}