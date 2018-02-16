namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Neusta.Shared.ObjectProvider.Internal;

	internal class ImplementationTypeSelector : IImplementationTypeSelector, ISelector
	{
		private readonly ITypeSourceSelector inner;
		private readonly IEnumerable<Type> types;
		private readonly List<ISelector> selectors = new List<ISelector>();

		public ImplementationTypeSelector(ITypeSourceSelector inner, IEnumerable<Type> types)
		{
			this.inner = inner;
			this.types = types;
		}

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
			var types = this.GetNonAbstractClasses(publicOnly);
			if (action != null)
			{
				var filter = new ImplementationTypeFilter(types);
				action(filter);
				types = filter.Types;
			}
			return this.AddSelector(types);
		}

		#endregion

		#region Explicit Implementation of ISelector

		void ISelector.Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy)
		{
			if (this.selectors.Count == 0)
			{
				this.AddClasses();
			}
			foreach (var selector in this.selectors)
			{
				selector.Populate(strategyApplier, strategy);
			}
		}

		#endregion

		#region Private Methods

		private IServiceTypeSelector AddSelector(IEnumerable<Type> types)
		{
			var selector = new ServiceTypeSelector(this, types);
			this.selectors.Add(selector);
			return selector;
		}

		private IEnumerable<Type> GetNonAbstractClasses(bool publicOnly)
		{
			return this.types.Where(t => t.IsNonAbstractClass(publicOnly));
		}

		#endregion
	}
}