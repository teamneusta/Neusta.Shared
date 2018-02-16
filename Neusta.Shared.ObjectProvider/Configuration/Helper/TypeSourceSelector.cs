namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Neusta.Shared.ObjectProvider.Internal;

	public class TypeSourceSelector : ITypeSourceSelector, ISelector
	{
		private readonly Assembly applicationAssembly;
		private readonly List<ISelector> selectors;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSourceSelector"/> class.
		/// </summary>
		public TypeSourceSelector()
		{
			this.selectors = new List<ISelector>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSourceSelector"/> class.
		/// </summary>
		public TypeSourceSelector(Assembly applicationAssembly)
			: this()
		{
			this.applicationAssembly = applicationAssembly;
		}

		#region Explicit Implementation of IAssemblySelector

		Assembly IAssemblySelector.InternalGetApplicationAssembly()
		{
			return this.applicationAssembly;
		}

		IImplementationTypeSelector IAssemblySelector.InternalFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			return this.AddSelector(assemblies.SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType()));
		}

		IImplementationTypeSelector IAssemblySelector.InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos)
		{
			return this.AddSelector(typeInfos.Select(t => t.Assembly).Distinct().SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType()));
		}

		#endregion

		#region Explicit Implementation of ITypeSelector

		IServiceTypeSelector ITypeSelector.InternalAddTypes(IEnumerable<Type> types)
		{
			return this.AddSelector(types).AddClasses();
		}

		#endregion

		#region Explicit Implementation of ISelector

		void ISelector.Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy)
		{
			foreach (var selector in this.selectors)
			{
				selector.Populate(strategyApplier, strategy);
			}
		}

		#endregion

		#region Private Methods

		private IImplementationTypeSelector AddSelector(IEnumerable<Type> types)
		{
			var selector = new ImplementationTypeSelector(this, types);
			this.selectors.Add(selector);
			return selector;
		}

		#endregion
	}
}